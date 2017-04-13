using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Cudafy;
using Cudafy.Host;
using Cudafy.Translator;
using Dsu.Common.Utilities.ExtensionMethods;

namespace Dsu.Common.Utilities.Cudafy
{
    public static class EmCudafy
    {
        /// <summary>
        /// GPU devices 를 반환
        /// e.g: var devices = eGPUType.Cuda.EnumerateDevices();
        /// </summary>
        /// <param name="gpuType"></param>
        /// <returns></returns>
        public static IEnumerable<GPGPU> EnumerateDevices(this eGPUType gpuType)
        {
            int cnt = CudafyHost.GetDeviceCount(gpuType);
            for (int i = 0; i < cnt; i++)
                yield return CudafyHost.GetDevice(gpuType, i);
        }


        public static eLanguage GetLanguage(this eGPUType type)
        {
            switch (type)
            {
                case eGPUType.Emulator:
                case eGPUType.Cuda:
                    return eLanguage.Cuda;
             
                case eGPUType.OpenCL:
                    return eLanguage.OpenCL;
            }
            throw new UnexpectedCaseOccurredException("Error on GPU type selection");
        }

        public static void InitializeGpu(eGPUType gpuType = eGPUType.Cuda)
        {
            CudafyModes.Target = gpuType;
            CudafyModes.DeviceId = 0;
            CudafyTranslator.Language = gpuType.GetLanguage();
        }

        public static IEnumerable<Tuple<string, object>> EnumerateDeviceProperties(this eGPUType gpuType)
        {
            var devices = gpuType.EnumerateDevices();

            foreach (var gpu in devices)
            {
                var props = gpu.GetDeviceProperties();

                Trace.WriteLine($"Available GPU : {props.Name}");
                Trace.WriteLine($"Available Devices : {CudafyHost.GetDeviceCount(gpuType)}");

                yield return new Tuple<string, object>("GraphicCardName", props.Name);
                yield return new Tuple<string, object>("NumGraphicCard", CudafyHost.GetDeviceCount(gpuType));

                foreach (PropertyDescriptor desc in TypeDescriptor.GetProperties(props))
                {
                    var value = desc.GetValue(props);
                    var dim3 = value as global::Cudafy.dim3;
                    if ( dim3 == null )
                        yield return new Tuple<string, object>(desc.Name, value);
                    else
                        yield return new Tuple<string, object>(desc.Name, String.Format("X={0}, Y={1}, Z={2}", dim3.x, dim3.y, dim3.z));
                }
            }            
        }



        private static bool IsCCompilerAvailable()
        {
            string clPath = Tools.FindExePath("cl.exe");
            return clPath.NonNullAny();
        }


        /*
         * Cuda file 내의 절대 파일 경로를 End user 의 path 에 맞게 변환 : e.g  <AssemblyPath>X:\WorkingSVN\server3d\bCudafy\App\bin\Release\Sharp3d.SceneGraph.dll</AssemblyPath>
         */
        /// <summary>
        /// Cuda file 내의 절대 파일 경로를 End user 의 path 에 맞게 변환
        /// </summary>
        /// <param name="cudafyFile"></param>
        private static void ModifyCompiledCudaPath(string cudafyFile)
        {
            bool modified = false;
            // 현재 exe 의 실행 path
            var dir = AppDomain.CurrentDomain.BaseDirectory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
            using(new CwdChanger(dir))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(cudafyFile);
                var nodes = doc.SelectNodes("//*[contains(local-name(), 'AssemblyPath')]");
                foreach (var a in nodes.Cast<XmlNode>())
                {
                    var pathNode = a.FirstChild;    // <AssemblyPath>XXX</AssemblyPath> : AssemblyPath tag 안쪽의 XXX 부분
                    var assemblyDir = Path.GetDirectoryName(pathNode.Value);
                    var assemblyFile = Path.GetFileName(pathNode.Value);
                    if (String.Compare(dir, assemblyDir, StringComparison.InvariantCultureIgnoreCase) != 0)
                    {
                        modified = true;
                        pathNode.Value = Path.Combine(dir, assemblyFile);
                    }
                }

                if (modified)
                    doc.Save(cudafyFile);
            }
        }



        private static bool _cudaCompileFailedOnThisApp = false;

        public static GPGPU GpuCompile(eGPUType gpuType, params Type[] t)
        {
            return GpuCompile(gpuType, gpuType.GetLanguage(), t);
        }
        public static GPGPU GpuCompile(eGPUType gpuType, eLanguage gpuLang, params Type[] t)
        {
            if (_cudaCompileFailedOnThisApp)
                return null;

            try
            {
                var gpu = CudafyHost.GetDevice(gpuType);
                CudafyTranslator.Language = gpuLang;
                CudafyModule km = CudafyTranslator.Cudafy(t);
                //CudafyModule km = null;
                //if ( gpuLang == eLanguage.Cuda )
                //    km = CudafyTranslator.Cudafy(eArchitecture.sm_35, t);
                //km = CudafyTranslator.Cudafy(t);
                gpu.LoadModule(km);
                return gpu;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to compile CUDA: " + ex.Message);
                _cudaCompileFailedOnThisApp = true;
                return null;
            }
        }



        /// <summary>
        /// GPGPU 가 Cell 별로 각기 하나의 multiple instance 로 동작하기 위해서 항상 새로 생성하도록 하기 위함
        /// https://cudafy.codeplex.com/wikipage?title=Multi-GPUs%20and%20context%20switching&referringTitle=Documentation
        /// </summary>
        /// <param name="gpuType"></param>
        /// <param name="startDeviceId">물리적 device id 임.</param>
        /// <returns></returns>
        private static GPGPU CreateCudaDevice(eGPUType gpuType, int startDeviceId=0)
        {
            //while (CudafyHost.DeviceCreated(gpuType, startDeviceId))
            //    startDeviceId++;

            return CudafyHost.GetDevice(gpuType, startDeviceId);
        }

        /// <summary>
        /// 주어진 type 들을 cudafy 한다.
        /// 
        /// 고려 사항
        /// <br/> - End user 는 Cudafy 환경이 갖추어져 있지 않으므로 compile 된 .cdfy 가 제공되어야 한다.
        /// <br/> - .cdfy 에는 절대 path 경로를 포함하고 있다. oops ...
        /// </summary>
        /// <param name="programName"></param>
        /// <param name="t"></param>
        /// <returns></returns>
        private static GPGPU GpuCompile(eGPUType gpuType, string compiledCuda, params Type[] t)
        {
            if (_cudaCompileFailedOnThisApp)
                return null;

            try
            {
                var gpu = CreateCudaDevice(gpuType);

                if (!compiledCuda.ToLower().EndsWith(".cdfy"))
                    compiledCuda += ".cdfy";

                string programName = Path.Combine(CommonApplication.GetProfilePath(), compiledCuda);

                CudafyModule km = null;
                if (!IsCCompilerAvailable() || CommonApplication.IsInstalledVersion)
                {
                    if (!File.Exists(programName))
                    {
                        var assembly = Assembly.GetEntryAssembly();
                        if (assembly == null)
                        {
                            // unit test 등에서 호출될 때에는 assembly 가 null 이 됨
                            return null;
                        }

                        programName = Path.Combine(Path.GetDirectoryName(assembly.Location), compiledCuda);
                    }

                    /*
                     * End-user version
                     */
                    ModifyCompiledCudaPath(programName);
                    km = CudafyModule.Deserialize(programName);
                }
                else
                {
                    /*
                     * Developer version
                     */
                    CudafyTranslator.Language = gpuType.GetLanguage();
                    CudafyTranslator.WorkingDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    // http://www.codeproject.com/Articles/202792/Using-Cudafy-for-GPGPU-Programming-in-NET
                    km = CudafyModule.TryDeserialize(programName);
                    if (km == null || !km.TryVerifyChecksums())
                    {
                        /*
                     * Cuda file is being compiled...
                     */
                        km = CudafyTranslator.Cudafy(t);
                        km.Serialize(programName);
                    }
                }

                gpu.LoadModule(km);
                return gpu;
            }
            catch (Exception ex)
            {
                _cudaCompileFailedOnThisApp = true;
                if (ex is DllNotFoundException)
                {
                    /*
                     * CUDA 관련 dll 이 존재하지 않는 경우 : Intel 계열 등에서 발생할 수 있음
                     * todo : OpenCL
                     */
                }

                MessageBox.Show("Failed to compile CUDA: " + ex.Message + "\r\n" + ex.StackTrace);
                return null;
            }
        }


        /// <summary>
        /// http://www.codeproject.com/Articles/582494/GPGPUplusPerformanceplusTests
        /// </summary>
        /// <param name="programName">Cudafy file name in xml format</param>
        /// <param name="t">class object whose module will be cudafied</param>
        /// <returns></returns>
        private static GPGPU NewGpu(eGPUType gpuType, string programName, params Type[] t)
        {
            var types = t.ToList();
            types.AddRange(new[] { typeof(EmCudafy) });
            Type[] typeArray = types.ToArray();

            if ( programName.NonNullAny())
                return GpuCompile(gpuType, programName, typeArray);            
            
            return GpuCompile(gpuType, typeArray);
        }

        private static GPGPU NewGpuEmulator(eLanguage gpuLang, params Type[] t)
        {
            var types = t.ToList();
            types.AddRange(new[] { typeof(EmCudafy) });
            Type[] typeArray = types.ToArray();

            return GpuCompile(eGPUType.Emulator, gpuLang, typeArray);
        }


        public static GPGPU NewCuda(string programName, params Type[] t) { return NewGpu(eGPUType.Cuda, programName, t); }
        public static GPGPU NewCuda(params Type[] t) { return NewGpu(eGPUType.Cuda, null, t); } 

        /// <summary>
        /// http://www.codeproject.com/Articles/582494/GPGPUplusPerformanceplusTests
        /// </summary>
        /// <returns></returns>
        public static GPGPU NewOpenCL(string programName, params Type[] t) { return NewGpu(eGPUType.OpenCL, programName, t); }
        public static GPGPU NewOpenCL(params Type[] t) { return NewGpu(eGPUType.OpenCL, null, t); }

        //public static GPGPU NewEmulator(string programName, params Type[] t) { return NewGpu(eGPUType.Emulator, programName, t); }
        public static GPGPU NewEmulator(eLanguage gpuLang, params Type[] t) { return NewGpuEmulator(gpuLang, t); }


        public static int ComputeBlockDimension(int numWorkPiece, int numThreadDimension)
        {
            return (numWorkPiece + numThreadDimension - 1)/numThreadDimension;
        }

        /// <summary>
        /// http://www.codeproject.com/Articles/276993/Base-Encoding-on-a-GPU
        /// </summary>
        /// <param name="thread"></param>
        /// <returns></returns>
        [Cudafy]
        public static int ComputeDimensionX(GThread thread)
        {
            return thread.blockIdx.x * thread.blockDim.x + thread.threadIdx.x;
        }
        [Cudafy]
        public static int ComputeDimensionY(GThread thread)
        {
            return thread.blockIdx.y * thread.blockDim.y + thread.threadIdx.y;
        }
        [Cudafy]
        public static int ComputeDimensionZ(GThread thread)
        {
            return thread.blockIdx.z * thread.blockDim.z + thread.threadIdx.z;
        }


        public static T[] PushToDevice<T>(this GPGPU gpu, T hostValue) where T : struct
        {
            T[] hostArray = new T[] { hostValue };
            return PushToDevice(gpu, hostArray);
        }

        public static T[] PushToDevice<T>(this GPGPU gpu, T[] hostArray) where T : struct
        {
            T[] devArray = gpu.Allocate<T>(hostArray);
            gpu.CopyToDevice(hostArray, 0, devArray, 0, hostArray.Length);
            return devArray;
        }

        /// <summary>
        /// Enumerable 을 device memory 에 복사한다.   enumerable 을 array 로 변환하는 과정으로 인해서 비효율적임.
        /// </summary>
        public static T[] PushEnumerableToDevice<T>(this GPGPU gpu, IEnumerable<T> hostEnumerables) where T : struct
        {
            return PushToDevice(gpu, hostEnumerables.ToArray());
        }



        /// <summary>
        /// Device array 를 읽어서 반환하는데, 반환 전에 device 의 memory 를 해제한다.
        /// length 는 할당된 device 의 memory size 와 일치(혹은 더 커야)해야 한다.
        /// </summary>
        public static T[] PullFromDevice<T>(this GPGPU gpu, T[] devArray, int length) where T : struct
        {
            Contract.Requires(length > 0);
            T[] hostArray = ReadFromDevice(gpu, devArray, length);
            gpu.Free(devArray);
            return hostArray;
        }

        /// <summary>
        /// Device array 를 읽어서 반환한다.
        /// length 는 할당된 device 의 memory size 와 일치(혹은 더 커야)해야 한다.
        /// </summary>
        public static T[] ReadFromDevice<T>(this GPGPU gpu, T[] devArray, int length) where T : struct
        {
            Contract.Requires(length > 0);
            T[] hostArray = new T[length];
            gpu.CopyFromDevice<T>(devArray, hostArray);
            return hostArray;
        }

        /// <summary>
        /// Device array 의 [start ~ end) 영역을 읽어서 반환한다.
        /// </summary>
        public static T[] ReadPartialFromDevice<T>(this GPGPU gpu, T[] devArray, int start, int end) where T : struct
        {
            Contract.Requires(end >= start);
            int count = end - start;
            T[] hostArray = new T[count];
            gpu.CopyFromDevice<T>(devArray, start, hostArray, 0, count);
            return hostArray;
        }
    }
}
