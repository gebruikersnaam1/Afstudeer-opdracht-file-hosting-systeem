using System;
using System.Collections.Generic;


namespace ProofOfConceptServer.entities.dummy_data
{
    public class DummyDataBlobfiles
    {

        public static List<BlobItem> GetDummyData () {
            return DummyDataBlobfiles.FilesStorage; 
        }

        private static List<BlobItem> FilesStorage = new List<BlobItem> {
            new BlobItem
            {
                FileId = 1,
                FileName="Technisch ontwerp",
                Date= Convert.ToDateTime("09-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize= 500,
                UserId="1",
                Description = "Een  ontwerp gemaakt door een UI-expert!"
            },
             new BlobItem
            {
                FileId = 2,
                FileName="Notules",
                Date= Convert.ToDateTime("12-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize= 700,
                UserId="2",
                Description = "Belangrijke notules van een belangrijk gesprek!"
            },
              new BlobItem
            {
                FileId = 3,
                FileName="Wetenschappelijk tijdschrift",
                Date=Convert.ToDateTime("13-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize= 500,
                UserId="3",
                Description = "Gemaakt door iemand die VWO heeft gedaan en te goed was voor TU Delft"
            },
               new BlobItem
            {
                FileId = 4,
                FileName="Functioneel ontwerp",
                Date=Convert.ToDateTime("13-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize=1500,
                UserId="4",
                Description = "Heel functioneel! Iemand zal het wellicht eens kunnen gebruiken!"
            },
            new BlobItem
            {
                FileId = 5,
                FileName="Verslag",
                Date= Convert.ToDateTime("13-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize=1300,
                UserId="5",
                Description = "Een wild verslag van dingen van dingen!"
            },
             new BlobItem
            {
                FileId = 6,
                FileName="Contract",
                Date=Convert.ToDateTime("14-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize=500,
                UserId="6",
                Description = "UFC contract voor het gevecht van het jaar!"
            },
             new BlobItem
            {
                FileId = 7,
                FileName="Feedback",
                Date=Convert.ToDateTime("14-10-2020"),
                PathFile="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize=500,
                UserId="4",
                Description = "Feedback of nee toch niet?"
            }
        };
    }
}
