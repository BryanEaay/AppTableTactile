using System;
using System.Windows.Input;

namespace CommonSurface.Utils
{
    //cette classe implementant l'interface ICommand va nous permette de binder des ICommand sur des bouton afin de déclencher des fonction contenu dans le ViewModel
    public class DelegateCommand : ICommand
    {
        #region Private Fields

        private Func<object, bool> canExecute;
        private bool canExecuteCache;
        private Action<object> executeAction;

        #endregion Private Fields

        #region Public Constructors

        //Methode qui sera appeler Sur un ICommand du ViewModel afin de pouvoir lancer une methode lors du click sur un bouton de la vue, il faut préciser un test et une fonction a executer
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

        #endregion Public Constructors

        #region Public Events

        public event EventHandler CanExecuteChanged;

        #endregion Public Events

        #region Public Methods

        public bool CanExecute(object parameter)
        {
            bool temp = canExecute(parameter);

            if (canExecuteCache != temp)
            {
                canExecuteCache = temp;
                if (CanExecuteChanged != null)
                {
                    //Ligne a enlever ou a mettre si on veux permettre le disable/enable automatique des boutons
                    CanExecuteChanged(this, new EventArgs());
                }
            }
            return canExecuteCache;
        }

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }

        #endregion Public Methods
    }
}