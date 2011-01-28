using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SelfhostedServer.Services {

        public class W3CPatternLayout : log4net.Layout.PatternLayout {
            public W3CPatternLayout() {
                ConversionPattern = "%message" + Environment.NewLine;
            }
            public override string Header {
                get {
                    return "#Software: SelfHostedServer " + Environment.NewLine
                        + "#Version 1.0" + Environment.NewLine
                        + "#Date: " + DateTime.Today.ToString("yyyy-MM-dd") + Environment.NewLine
                        + "#Fields: time c-ip cs-method cs-uri-stem sc-status time-taken bytes" + Environment.NewLine;
                }
                set {

                }
            }

            public override string Footer {
                get {
                    return Environment.NewLine;
                }
                set {

                }
            }
        }


}
