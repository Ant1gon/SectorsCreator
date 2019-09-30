﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SectorsCreator
{
	class Program
	{
		static void Main(string[] args)
		{
			string _SEPARATOR = "****************************";
			double.TryParse(ConfigurationManager.AppSettings.Get("start_lat").Trim().Replace(".",","), out double start_lat);
			double.TryParse(ConfigurationManager.AppSettings.Get("start_long").Trim().Replace(".", ","), out double start_long);
			double.TryParse(ConfigurationManager.AppSettings.Get("end_lat").Trim().Replace(".", ","), out double end_lat);
			double.TryParse(ConfigurationManager.AppSettings.Get("end_long").Trim().Replace(".", ","), out double end_long);

			double change_Y = start_lat>end_lat? (start_lat - end_lat) / 5: (end_lat - start_lat) / 5;
			double change_X = start_long>end_long? (start_long - end_long ) / 5: (end_long - start_long) / 5;			

			double tLong = start_long, tLat = start_lat;

			
			Dictionary<string, Sector> sectors = new Dictionary<string, Sector>();
			List<string> coordinates = new List<string>();

			int counter = 0;

			for(int y = 0; y < 5; y++){
				for (int x = 0; x < 5; x++){
					counter++;
					Sector sector = new Sector();
					sector.start_lat = tLat + change_Y * y;
					sector.start_long = tLong + change_X * x;
					sector.end_lat = tLat + (change_Y * (y + 1));
					sector.end_long = tLong + (change_X * (x + 1));

					sectors.Add(counter.ToString(), sector);
					coordinates.Add(string.Format("Sector {0} :: {1}",counter, sector.getCoordinates(sector)));
				}
			}
			string logfile = string.Format("{0}\\Sectors.txt", Environment.CurrentDirectory);
			File.AppendAllText(logfile,string.Format("{0}{1}{2}{1}",_SEPARATOR,Environment.NewLine,string.Join(Environment.NewLine,coordinates)));
			Console.WriteLine("The calculation is complete.\nPress any key to continue.");
			Console.ReadKey();
		}
	}
	class Sector
	{
		public double start_lat { get; set; }
		public double start_long { get; set; }
		public double end_lat { get; set; }
		public double end_long { get; set; }

		public string getCoordinates(Sector sector)
		{
			return string.Format("Lat: {0} Long: {1}//Lat: {2} Long: {3}", 
				sector.start_lat,
				sector.start_long,
				sector.end_lat,
				sector.end_long);
		}
	}
}
