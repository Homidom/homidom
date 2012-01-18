using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using HoMIDroid.Controllers;

namespace HoMIDroid.BO
{
    public class Group<T> : BaseObject, INamedObject where T : BaseObject
    {
        /// <summary>
        /// Gets the group name
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the group description
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Gets the list of devices in this group.
        /// </summary>
        public List<T> Elements { get; private set; }

        private Group()
        {
            this.Elements = new List<T>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group"/> class.
        /// </summary>
        /// <param name="category">The category of devices contained in this group.</param>
        public Group(DeviceCategory category)
            : this()
        {
            switch(category)
            {
                case DeviceCategory.Energy:
                    this.Name = "�nergie";
                    break;
                case DeviceCategory.Light:
                    this.Name = "Lampes";
                    break;
                case DeviceCategory.Meteo:
                    this.Name = "M�t�o";
                    break;
                case DeviceCategory.Multimedia:
                    this.Name = "Multim�dia";
                    break;
                case DeviceCategory.Switch:
                    this.Name = "Interrupteurs";
                    break;
                default:
                    this.Name = "Autre";
                    break;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Group&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="name">The group name.</param>
        public Group(string name)
            : this()
        {
            this.Name = name;
        }


        public override Controllers.BaseController GetController(Context context)
        {
            return new NamedController<Group<T>>(context, this);
        }
    }
}