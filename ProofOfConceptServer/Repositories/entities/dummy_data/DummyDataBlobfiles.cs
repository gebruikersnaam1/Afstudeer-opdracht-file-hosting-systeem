using System;
using System.Collections.Generic;
using ProofOfConceptServer.Repositories.entities;

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
                Path="https://lostclouds.com/2Communicate/project/SRS.pdf",
                FileSize= 500,
                UserId="1",
                Description = "Een  ontwerp gemaakt door een UI-expert!"
            },
             new BlobItem
            {
                FileId = 2,
                FileName="Notules",
                Date= Convert.ToDateTime("12-10-2020"),
                Path="https://lostclouds.com/2Communicate/project/SRS.pdf",
                FileSize= 700,
                UserId="2",
                Description = "Belangrijke notules van een belangrijk gesprek!"
            },
              new BlobItem
            {
                FileId = 3,
                FileName="Wetenschappelijk tijdschrift",
                Date=Convert.ToDateTime("13-10-2020"),
                Path="https://lostclouds.com/2Communicate/project/SRS.pdf",
                FileSize= 500,
                UserId="3",
                Description = "Gemaakt door iemand die VWO heeft gedaan en te goed was voor TU Delft"
            },
               new BlobItem
            {
                FileId = 4,
                FileName="Functioneel ontwerp",
                Date=Convert.ToDateTime("13-10-2020"),
                Path="https://lostclouds.com/2Communicate/project/SRS.pdf",
                FileSize=1500,
                UserId="4",
                Description = "Heel functioneel! Iemand zal het wellicht eens kunnen gebruiken!"
            },
            new BlobItem
            {
                FileId = 5,
                FileName="Verslag",
                Date= Convert.ToDateTime("13-10-2020"),
                Path="https://lostclouds.com/2Communicate/project/SRS.pdf",
                FileSize=1300,
                UserId="5",
                Description = "Een wild verslag van dingen van dingen!"
            },
             new BlobItem
            {
                FileId = 6,
                FileName="Contract",
                Date=Convert.ToDateTime("14-10-2020"),
                Path="https://19of32x2yl33s8o4xza0gf14-wpengine.netdna-ssl.com/wp-content/uploads/Exhibit-A-SAMPLE-CONTRACT.pdf",
                FileSize=500,
                UserId="6",
                Description = "UFC contract voor het gevecht van het jaar!"
            },
             new BlobItem
            {
                FileId = 7,
                FileName="Feedback",
                Date=Convert.ToDateTime("14-10-2020"),
                Path="https://www.open-overheid.nl/wp-content/uploads/2018/11/digitale-versie-vouwfolder.pdf",
                FileSize=500,
                UserId="4",
                Description = "Feedback of nee toch niet?"
            },
             new BlobItem
            {
                FileId = 8,
                FileName="Daphnis logo",
                Date= Convert.ToDateTime("11-11-2020"),
                Path="http://daphnis.nl/wp-content/uploads/revslider/TheGem%20Agency%20(One-Page)/Daphnis-2-wit.png",
                FileSize= 500,
                UserId="4",
                Description = "Super cool deze logo"
            },
             new BlobItem
            {
                FileId = 9,
                FileName="Flex4NL logo",
                Date= Convert.ToDateTime("12-11-2020"),
                Path="https://www.flex4nl.com/_images/redesign/logo.svg",
                FileSize= 3,
                UserId="1",
                Description = "Jij liegt, maar wij zijn cool!"
            }
        };
    }
}
