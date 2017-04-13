// Learn more about F# at http://fsharp.org. See the 'F# Tutorial' project
// for more guidance on F# programming.

#r @"C:\Program Files (x86)\National Instruments\MeasurementStudioVS2010\DotNET\Assemblies\Current\NationalInstruments.DAQmx.dll"
#load "Types.fs"

#I "../bin"
#r "Dsu.Common.Utilities.Core.dll"
#r "../../app/Kefico\Dsa.Kefico.Cp/bin/Dsu.Common.Utilities.FS.dll"
#load "Voltage.fs"

open NationalInstruments.DAQmx

// Define your library scripting code here

let range = -10.0, 10.0
let measured = Voltage.readSingleSample "dev1/ai1" AITerminalConfiguration.Differential range AIVoltageUnits.Volts
let value = measured.Value

