using System;
using System.Collections.Generic;
using System.IO;
using System.Reactive.Subjects;
using Dsu.Common.Interfaces;

namespace Dsu.Common.Utilities.MVCs
{
    public abstract class DocBase
        : IDocument
        , IDescribable
        , ISubscribable
    {
        public string Name { get; set; }
        public virtual string FilePath { get; set; }
        public Guid Guid { get; set; }

        public string Creator { get; set; }

        public Subject<IObservableDocumentEvent> DocumentChangeSubject = new Subject<IObservableDocumentEvent>();

        private List<IDisposable> _documentSubscriptions = new List<IDisposable>();
        public void AddSubscription(IDisposable subscription) { _documentSubscriptions.Add(subscription); }

        public void AddCreator(string creator)
        {
            if (String.IsNullOrEmpty(Creator))
                Creator = creator;
            else
                Creator = String.Format("{0};{1}", Creator, creator);
        }

        /// <summary> Program generated notes.  e.g Note="Creator=GLComConverter1.0;Date=20150410;" </summary>
        public string ProgramNote { get; set; }

        /// <summary> User provided free form description. comments. </summary>
        public string Note { get; set; }

        public string GuidString
        {
            get { return Guid.ToString(); }
            set { Guid = new Guid(value);}
        }

        protected bool? _dirty;

        public bool Dirty
        {
            get { return _dirty.GetValueOrDefault(); }
            set
            {
                if (!_dirty.HasValue)
                {
                    _dirty = value;
                    return;
                }

                if (_dirty.Value != value)
                {
                    _dirty = value;
                    OnSetDirty();
                }
            }
        }

        protected virtual void OnSetDirty()
        {
        }


        private static int _counter = 0;

        private SanityCheckableImpl _sanityCheckableImpl;

        protected DocBase(string filepath)
        {
            Name = String.IsNullOrEmpty(filepath) ? String.Format("Unnamed{0}", ++_counter) : Path.GetFileName(filepath);
            FilePath = filepath;
            Guid = Guid.NewGuid();
            _sanityCheckableImpl = new SanityCheckableImpl(this);
        }

        public virtual void InvokeModelChangedEventHandler(ModelChangedEventArgs args)
        {
            UpdateModels(this, args);
        }

        public abstract string GetDefaultFileExtension();

        public virtual void UpdateModels(object sender, ModelChangedEventArgs args)
        {
            if ( _updateProgressing )
                _pendingUpdate.Add(new Tuple<object, ModelChangedEventArgs>(sender, args));
            else
                DocumentChangeSubject.OnNext(args);
        }



        protected bool _updateProgressing = false;
        private List<Tuple<object, ModelChangedEventArgs>> _pendingUpdate = new List<Tuple<object, ModelChangedEventArgs>>();
        public void BeginUpdate()
        {
            if ( _updateProgressing )
                throw new UnexpectedCaseOccurredException("Update is already progressing.");

            _updateProgressing = true;
        }

        public void EndUpdate()
        {
            if (! _updateProgressing)
                throw new UnexpectedCaseOccurredException("No update progress exists.  Failed to end update.");

            _updateProgressing = false;

            UpdateModels(this, new ModelChangedEventArgs(reason:"EndUpdate"));
        }

        public class UpdateBeginer : IDisposable
        {
            private DocBase _doc;

            public UpdateBeginer(DocBase doc)
            {
                _doc = doc;
                doc.BeginUpdate();
            }
            public void Dispose()
            {
                _doc.EndUpdate();
            }
        }

        public UpdateBeginer NewUpdateBeginer() {  return new UpdateBeginer(this); }

        public abstract bool Save();

        public abstract bool SaveAs(string filePath);


        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        ~DocBase() { Dispose(false); } 
        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                _documentSubscriptions.ForEach(s => s.Dispose());
                _documentSubscriptions.Clear();
            }

            // release any unmanaged objects
            // set the object references to null

            _disposed = true;
        }

        protected virtual string CheckSanityLocal() { return String.Empty; }
        public string CheckSanity(bool stopOnFirstError = true)
        {
            return CheckSanityLocal() + _sanityCheckableImpl.CheckSanity(stopOnFirstError);
        }
       
        public abstract IEnumerable<ISanityCheckable> SanityCheckables { get; }
    }
}
