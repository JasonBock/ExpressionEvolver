using System.Collections.ObjectModel;

namespace DataVisualizationDemos
{
    public class EngineMeasurementCollection : Collection<EngineMeasurement>
    {
        public EngineMeasurementCollection()
        {
            Add(new EngineMeasurement { Speed = 1000, Torque = 100, Power = 20 });
            Add(new EngineMeasurement { Speed = 2000, Torque = 160, Power = 60 });
            Add(new EngineMeasurement { Speed = 3000, Torque = 210, Power = 125 });
            Add(new EngineMeasurement { Speed = 4000, Torque = 220, Power = 160 });
            Add(new EngineMeasurement { Speed = 5000, Torque = 215, Power = 205 });
            Add(new EngineMeasurement { Speed = 6000, Torque = 200, Power = 225 });
            Add(new EngineMeasurement { Speed = 7000, Torque = 170, Power = 200 });
        }
    }
}
