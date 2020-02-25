using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace AppAdministrationWPF.Utils
{
    //cette classe implementant l'interface ICommand va nous permette de binder des ICommand sur des bouton afin de déclencher des fonction contenu dans le ViewModel
    public class DelegateCommand : ICommand
    {
        Func<object, bool> canExecute;
        Action<object> executeAction;
        bool canExecuteCache;

        //Methode qui sera appeler Sur un ICommand du ViewModel afin de pouvoir lancer une methode lors du click sur un bouton de la vue, il faut préciser un test et une fonction a executer
        public DelegateCommand(Action<object> executeAction, Func<object, bool> canExecute)
        {
            this.executeAction = executeAction;
            this.canExecute = canExecute;
        }

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

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            executeAction(parameter);
        }
    }
}
