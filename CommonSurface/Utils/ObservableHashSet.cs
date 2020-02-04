using System.Collections.ObjectModel;

namespace CommonSurface.Utils
{
    public class ObservableHashSet<T> : ObservableCollection<T>
    {
        #region Public Constructors

        public ObservableHashSet(ObservableHashSet<T> copy)
            : base(copy) { }

        public ObservableHashSet() : base()
        {
        }

        #endregion Public Constructors

        #region Protected Methods

        protected override void InsertItem(int index, T item)
        {
            if (Contains(item))
            {
                return;
            }
            base.InsertItem(index, item);
        }

        protected override void SetItem(int index, T item)
        {
            int i = IndexOf(item);
            if (i >= 0 && i != index) return;

            base.SetItem(index, item);
        }

        #endregion Protected Methods
    }
}