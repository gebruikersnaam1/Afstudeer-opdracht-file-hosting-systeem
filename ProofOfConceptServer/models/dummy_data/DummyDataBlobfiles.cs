using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProofOfConceptServer
{
    public class DummyDataBlobfiles
    {

        public static List<BlobItem> GetDummyData () {
            return DummyDataBlobfiles.FilesStorage; 
        }

        private static List<BlobItem> FilesStorage = new List<BlobItem> {
            new BlobItem
            {
                fileId = "1",
                fileName="Technisch ontwerp",
                date="09-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="500kb",
                userId="1",
                description = "Een  ontwerp gemaakt door een UI-expert!"
            },
             new BlobItem
            {
                fileId = "2",
                fileName="Notules",
                date="12-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="700kb",
                userId="2",
                description = "Belangrijke notules van een belangrijk gesprek!"
            },
              new BlobItem
            {
                fileId = "3",
                fileName="Wetenschappelijk tijdschrift",
                date="13-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="500kb",
                userId="3",
                description = "Gemaakt door iemand die VWO heeft gedaan en te goed was voor TU Delft"
            },
               new BlobItem
            {
                fileId = "4",
                fileName="Functioneel ontwerp",
                date="13-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="1500kb",
                userId="4",
                description = "Heel functioneel! Iemand zal het wellicht eens kunnen gebruiken!"
            },
            new BlobItem
            {
                fileId = "5",
                fileName="Verslag",
                date="13-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="1300kb",
                userId="5",
                description = "Een wild verslag van dingen van dingen!"
            },
             new BlobItem
            {
                fileId = "6",
                fileName="Contract",
                date="14-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="500kb",
                userId="6",
                description = "UFC contract voor het gevecht van het jaar!"
            },
             new BlobItem
            {
                fileId = "7",
                fileName="Feedback",
                date="14-10-2020",
                pathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                fileSize="500kb",
                userId="4",
                description = "Feedback of nee toch niet?"
            }
        };
    }
}
