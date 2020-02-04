using CommonSurface.Model;
using CommonSurface.Other;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;

namespace CommonSurface.XML
{
    public class XMLProvider
    {
        #region Private Fields

        private static XMLProvider instance;
        private String cheminFichier = ConfigurationManager.AppSettings["cheminXmlConf"];
        private XDocument document;

        #endregion Private Fields

        #region Private Constructors

        private XMLProvider()
        {
            if (!Directory.Exists(Path.GetDirectoryName(cheminFichier)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(cheminFichier));
            }
            try
            {
                document = XDocument.Load(cheminFichier);
            }
            catch (FileNotFoundException)
            {
                if (MessageBox.Show("Le fichier XML n'a pas pu être trouvé un nouveau va être crée voulez vous continuer ? ", "Fichier XML non trouvé", MessageBoxButtons.OK, MessageBoxIcon.Warning) == DialogResult.OK
                    )
                {
                    Initialiser();
                }
            }
        }

        #endregion Private Constructors

        #region Public Properties

        public static XMLProvider Provider
        {
            get
            {
                if (instance == null)
                {
                    instance = new XMLProvider();
                }
                return instance;
            }
        }

        #endregion Public Properties

        #region Public Methods

        public void Initialiser()
        {
            if (document == null)
            {
                document = new XDocument(
                    new XElement("AppPalaisDesRois",
                        new XElement("Mediatheque",
                                new XElement("Media", new XAttribute("Id", 0), new XAttribute("X", 0), new XAttribute("Y", 0), new XAttribute("Type", "Defaut"), new XAttribute("Nom", "DEFAUT"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Section", "DEFAUT"))
                                    ),
                        new XElement("Memory",
                            new XElement("Jeu", new XAttribute("Id", 1), new XAttribute("taille", 16),
                                new XElement("Carte", new XAttribute("Id", 1), new XAttribute("Nom", "laCarte11"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 2), new XAttribute("Nom", "laCarte12"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 3), new XAttribute("Nom", "laCarte13"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 4), new XAttribute("Nom", "laCarte14"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 5), new XAttribute("Nom", "laCarte15"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 6), new XAttribute("Nom", "laCarte16"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 7), new XAttribute("Nom", "laCarte17"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 8), new XAttribute("Nom", "laCarte18"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut))
                                        ),
                            new XElement("Jeu", new XAttribute("Id", 2), new XAttribute("taille", 16),
                                new XElement("Carte", new XAttribute("Id", 1), new XAttribute("Nom", "laCarte21"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 2), new XAttribute("Nom", "laCarte22"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 3), new XAttribute("Nom", "laCarte23"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 4), new XAttribute("Nom", "laCarte24"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 5), new XAttribute("Nom", "laCarte25"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 6), new XAttribute("Nom", "laCarte26"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 7), new XAttribute("Nom", "laCarte27"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 8), new XAttribute("Nom", "laCarte28"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut))
                                        ),
                             new XElement("Jeu", new XAttribute("Id", 3), new XAttribute("taille", 36),
                                new XElement("Carte", new XAttribute("Id", 1), new XAttribute("Nom", "laCarte31"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 2), new XAttribute("Nom", "laCarte32"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 3), new XAttribute("Nom", "laCarte33"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 4), new XAttribute("Nom", "laCarte34"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 5), new XAttribute("Nom", "laCarte35"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 6), new XAttribute("Nom", "laCarte36"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 7), new XAttribute("Nom", "laCarte37"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 8), new XAttribute("Nom", "laCarte38"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 9), new XAttribute("Nom", "laCarte39"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 10), new XAttribute("Nom", "laCarte310"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 11), new XAttribute("Nom", "laCarte311"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 12), new XAttribute("Nom", "laCarte312"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 13), new XAttribute("Nom", "laCarte313"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 14), new XAttribute("Nom", "laCarte314"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 15), new XAttribute("Nom", "laCarte315"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 16), new XAttribute("Nom", "laCarte316"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 17), new XAttribute("Nom", "laCarte317"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut)),
                                new XElement("Carte", new XAttribute("Id", 18), new XAttribute("Nom", "laCarte318"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut))
                                        )
                                    ),
                        new XElement("Puzzle",
                            new XElement("Jeu", new XAttribute("Id", "1"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "6"), new XAttribute("Type", "IMAGE")
                                        ),
                            new XElement("Jeu", new XAttribute("Id", "2"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "6"), new XAttribute("Type", "IMAGE")
                                        ),
                            new XElement("Jeu", new XAttribute("Id", "3"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "6"), new XAttribute("Type", "IMAGE")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "4"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "6"), new XAttribute("Type", "VIDEO")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "5"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "6"), new XAttribute("Type", "VIDEO")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "6"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "IMAGE")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "7"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "IMAGE")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "8"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "IMAGE")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "9"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "VIDEO")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "10"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "VIDEO")
                            ),
                            new XElement("Jeu", new XAttribute("Id", "11"), new XAttribute("Nom", "defaut"), new XAttribute("Chemin", MediaManager.cheminMediaDefaut), new XAttribute("Taille", "12"), new XAttribute("Type", "VIDEO")
                            )

                                    )/*Fermeture Puzzle*/
                            )/*Fermeture AppPalaisDesRois*/
                            );/*Fermeture document*/

                Console.WriteLine(document);
                document.Save(cheminFichier);
            }
        }

        #endregion Public Methods

        #region ServiceFunctionMemory

        public int nbJeuMemory()
        {
            int retour = 0;
            IEnumerable<XElement> lesJeux = (from jeu in Provider.document.Root.Elements("Memory").Elements()
                                             select jeu);
            retour = lesJeux.Count();
            return retour;
        }

        public List<Carte> ObtenirCartesMemory(int IdJeu)
        {
            IEnumerable<XElement> lesJeux = (from jeu in Provider.document.Root.Elements("Memory").Elements()
                                             where jeu.Attribute("Id").Value.Equals(IdJeu.ToString())
                                             select jeu);

            XElement leJeu = null;
            foreach (XElement xe in lesJeux)
            {
                leJeu = xe;
            }
            List<Carte> retour = new List<Carte>();
            foreach (XElement xe in leJeu.Elements())
            {
                retour.Add(new Carte(int.Parse(xe.Attribute("Id").Value), xe.Attribute("Chemin").Value, xe.Attribute("Nom").Value));
            }
            return retour;
        }

        #endregion ServiceFunctionMemory

        #region ServiceFunctionPuzzle

        public void AjouterPuzzle(int taille, MediaType type, string chemin, string nom)
        {
            int id = 0;
            foreach (XElement e in Provider.document.Root.Element("Puzzle").Elements())
            {
                if (id < Convert.ToInt16(e.Attribute("Id").Value))
                {
                    id = Convert.ToInt16(e.Attribute("Id").Value);
                }
            }

            id++;

            XElement nouveau = new XElement("Jeu", new XAttribute("Id", id.ToString()), new XAttribute("Nom", nom), new XAttribute("Chemin", chemin), new XAttribute("Taille", taille.ToString()), new XAttribute("Type", type.ToString()));
            Provider.document.Root.Element("Puzzle").Add(nouveau);
            Provider.document.Save(cheminFichier);
        }

        public void ModifierMediaPuzzle(Media leMedia)
        {
            XElement jeuAModifier = ObtenirJeu(0);
            jeuAModifier.SetAttributeValue("Type", leMedia.Type);
            jeuAModifier.SetAttributeValue("Path", leMedia.Path);
            jeuAModifier.SetAttributeValue("Name", leMedia.Name);

            Provider.document.Save(cheminFichier);
        }

        public List<Media> ObtenirMediasPuzzle(int taille, MediaType type)
        {
            List<Media> retour = new List<Media>();

            IEnumerable<XElement> medias = (from jeu in Provider.document.Root.Element("Puzzle").Elements()
                                            where int.Parse(jeu.Attribute("Taille").Value).Equals(taille) &&
                                            Enum.Parse(typeof(MediaType), jeu.Attribute("Type").Value, true).Equals(type)
                                            select jeu);

            foreach (XElement xe in medias)
            {
                Media media = new Media(xe.Attribute("Nom").Value, xe.Attribute("Chemin").Value, (MediaType)Enum.Parse(typeof(MediaType), xe.Attribute("Type").Value, true));
                retour.Add(media);
            }

            return retour;
        }

        public void SupprimerPuzzle(int taille, MediaType type, int id)
        {
            foreach (XElement e in Provider.document.Root.Element("Puzzle").Elements())
            {
                if (id == Convert.ToInt16(e.Attribute("Id").Value))
                {
                    e.Remove();
                }
            }

            Provider.document.Save(cheminFichier);
        }

        private XElement ObtenirJeu(int IdJeu)
        {
            IEnumerable<XElement> lesJeux = (from jeux in Provider.document.Root.Element("Puzzle").Elements()
                                             where jeux.Attribute("Id").Value.Equals(IdJeu.ToString())
                                             select jeux);
            XElement retour = null;
            foreach (XElement xe in lesJeux)
            {
                retour = xe;
            }
            return retour;
        }

        #endregion ServiceFunctionPuzzle
    }
}