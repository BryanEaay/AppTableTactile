using System;
using System.Net;
using System.Windows;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using System.ComponentModel;

using CommonSurface.Model;
using CommonSurface.XML;

namespace AppAdministrationWPF.ViewModel
{
    public class AdminTimeLapseViewModel : GenericViewModel
    {
        public AdminTimeLapseViewModel(){
            CollectionsMedia = new ObservableCollection<Media>[3];
            CollectionNames = new string[] { "MediaJardin", "MediaPremier", "MediaSecond" };
            this.Type = MediaType.VIDEO;
            this.MediaJardin = new ObservableCollection<Media>(XMLProvider.Provider.ObtenirMediaTimeLapse(CommonSurface.Model.Section.JARDIN));
            this.MediaPremier = new ObservableCollection<Media>(XMLProvider.Provider.ObtenirMediaTimeLapse(CommonSurface.Model.Section.PREMIERETAGE));
            this.MediaSecond = new ObservableCollection<Media>(XMLProvider.Provider.ObtenirMediaTimeLapse(CommonSurface.Model.Section.DEUXIEMEETAGE));
		}

        public override void AddMedia()
        {
            //Media media = new Media(0, this.Tmp.X, this.Tmp.Y, this.Tmp.Chemin, this.Tmp.Nom, MediaType.IMAGE, this.Tmp.Section);
            XMLProvider.Provider.AjouteMediaTimeLapse(this.Tmp);
            this.SelectedTabIndex = this.SectionIndex;
            this.SelectedItem = this.Tmp;
            OnPropertyChanged("SelectedTabIndex");
            OnPropertyChanged(CollectionNames[this.SectionIndex]);
        }

        public override void EditMedia()
        {
            CommonSurface.Model.Section prev = this.SelectedItem.Section;
            CommonSurface.Model.Section next = this.Section;

            this.SelectedItem.Nom = this.Tmp.Nom;
            this.SelectedItem.X = this.Tmp.X;
            this.SelectedItem.Y = this.Tmp.Y;
            this.SelectedItem.Chemin = this.Chemin;
            this.SelectedItem.Section = this.Section;

            XMLProvider.Provider.ModifierMediaTimeLapse(this.SelectedItem);
            this.SelectedTabIndex = (int)next;
            OnPropertyChanged("SelectedTabIndex");
        }

        public override void SupprMedia()
        {

            XMLProvider.Provider.SupprimerMediaTimeLapse(this.SelectedItem);
            this.SelectedTabIndex = (int)this.SelectedItem.Section;
        }
    }
}
