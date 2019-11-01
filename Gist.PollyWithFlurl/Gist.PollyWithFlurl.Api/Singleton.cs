using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gist.PollyWithFlurl.Api
{
    public class Singleton
    {
        private static Singleton session { get; set; }

        public static Singleton GetSession
        {
            get
            {
                if (session is null)
                {
                    session = new Singleton();
                }

                return session;
            }
        }

        public int SuccessOnThirdTry { get; set; }

    }
}
