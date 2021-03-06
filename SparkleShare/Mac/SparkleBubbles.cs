//   SparkleShare, a collaboration and sharing tool.
//   Copyright (C) 2010  Hylke Bons <hylkebons@gmail.com>
//
//   This program is free software: you can redistribute it and/or modify
//   it under the terms of the GNU General Public License as published by
//   the Free Software Foundation, either version 3 of the License, or
//   (at your option) any later version.
//
//   This program is distributed in the hope that it will be useful,
//   but WITHOUT ANY WARRANTY; without even the implied warranty of
//   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//   GNU General Public License for more details.
//
//   You should have received a copy of the GNU General Public License
//   along with this program. If not, see <http://www.gnu.org/licenses/>.


using System;
using System.IO;

using MonoMac.AppKit;
using MonoMac.Foundation;
using MonoMac.Growl;

namespace SparkleShare {
    
    public class SparkleBubbles : NSObject {

        private SparkleBubblesController controller = new SparkleBubblesController ();


        public SparkleBubbles ()
        {
            this.controller.ShowBubbleEvent += delegate (string title, string subtext, string image_path) {
                InvokeOnMainThread (delegate {
                    if (!GrowlApplicationBridge.IsGrowlRunning ()) {
                        NSApplication.SharedApplication.RequestUserAttention (
                            NSRequestUserAttentionType.InformationalRequest);

                        return;
                    }

                    if (NSApplication.SharedApplication.DockTile.BadgeLabel == null) {
                        NSApplication.SharedApplication.DockTile.BadgeLabel = "1";

                    } else {
                        int events = int.Parse (NSApplication.SharedApplication.DockTile.BadgeLabel);
                        NSApplication.SharedApplication.DockTile.BadgeLabel = (events + 1).ToString ();
                    }

                    if (image_path != null && File.Exists (image_path)) {
                        NSData image_data = NSData.FromFile (image_path);
                        GrowlApplicationBridge.Notify (title, subtext,
                            "Event", image_data, 0, false, null);

                    } else {
                        GrowlApplicationBridge.Notify (title, subtext,
                            "Event", null, 0, false, null);
                    }
                });
            };
        }
    }
}
