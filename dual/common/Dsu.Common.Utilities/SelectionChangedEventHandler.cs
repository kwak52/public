using System;
using System.Collections.Generic;
using System.Linq;

using SelectionObjectType = System.Object;

namespace Dsu.Common.Utilities
{
    public enum ChangeType
    {
        Added,
        Removed,
        Modified,
    }

    public enum SelectionChangeType
    {
        /// <summary>
        /// selection set 에 추가됨
        /// </summary>
        Added,
        /// <summary>
        /// selection set 에서 제거 됨
        /// </summary>
        Removed,
    }


    public abstract class SelectionChangedEventArgs : EventArgs, IObservableUIEvent
    {
        public SelectionChangeType ChangeType { get; private set; }

        protected SelectionChangedEventArgs(SelectionChangeType type)
        {
            ChangeType = type;
        }

        public abstract IEnumerable<SelectionObjectType> GetAllSelections();
    }

    public class SingleSelectionChangedEventArgs : SelectionChangedEventArgs
    {
        public SelectionObjectType SelectionObject { get; private set; }

        public SingleSelectionChangedEventArgs(SelectionObjectType selectionObject, SelectionChangeType type) : base(type)
        {
            SelectionObject = selectionObject;
        }

        public SingleSelectionChangedEventArgs(SelectionObjectType selectionObject)
            : base(selectionObject == null ? SelectionChangeType.Removed : SelectionChangeType.Added)
        {
            SelectionObject = selectionObject;
        }


        public override IEnumerable<SelectionObjectType> GetAllSelections()
        {
            if ( SelectionObject == null )
                yield break;

            yield return SelectionObject;
        }
    }

    public class MultipleSelectionChangedEventArgs : SelectionChangedEventArgs
    {
        private List<SelectionObjectType> _selectionObjects = new List<SelectionObjectType>();
        public IEnumerable<SelectionObjectType> SelectionObjects { get { return _selectionObjects; } }

        public MultipleSelectionChangedEventArgs(IEnumerable<SelectionObjectType> selectionObjects, SelectionChangeType type)
            : base(type)
        {
            _selectionObjects = selectionObjects.ToList();
        }

        public override IEnumerable<SelectionObjectType> GetAllSelections() { return _selectionObjects; } 
    }
}
