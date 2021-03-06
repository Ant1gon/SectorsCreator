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
			double a = 11.23456789;
			Console.WriteLine(a.ToString("0.00"));
			Console.WriteLine(a.ToString("0.000"));
			Console.WriteLine(a.ToString("0.0000"));
			Console.WriteLine(a.ToString("0.00000"));
			Console.ReadLine();



			string _SEPARATOR = "****************************";
			double.TryParse(ConfigurationManager.AppSettings.Get("start_latitude").Trim().Replace(".",","), out double start_lat);
			double.TryParse(ConfigurationManager.AppSettings.Get("start_longitude").Trim().Replace(".", ","), out double start_long);
			double.TryParse(ConfigurationManager.AppSettings.Get("end_latitude").Trim().Replace(".", ","), out double end_lat);
			double.TryParse(ConfigurationManager.AppSettings.Get("end_longitude").Trim().Replace(".", ","), out double end_long);

			double change_Lat = start_lat>end_lat? (start_lat - end_lat) / 5: (end_lat - start_lat) / 5;
			double change_Long = start_long>end_long? (start_long - end_long ) / 5: (end_long - start_long) / 5;			

			double tLong = start_long, tLat = start_lat;

			
			Dictionary<string, Sector> sectors = new Dictionary<string, Sector>();
			List<string> coordinates = new List<string>();

			int counter = 0;

			for(int y = 0; y < 5; y++){
				for (int x = 0; x < 5; x++){
					counter++;
					Sector sector = new Sector();
					sector.start_latitude = tLat - change_Lat * y;
					sector.start_longitude = tLong + change_Long * x;
					sector.end_latitude = tLat - (change_Lat * (y + 1));
					sector.end_longitude = tLong + (change_Long * (x + 1));

					sectors.Add(counter.ToString(), sector);
					coordinates.Add(string.Format("Sector {0} :: {1}",counter, sector.getCoordinates(sector)));
				}
			}
			string logfile = string.Format("{0}\\Sectors.txt", Environment.CurrentDirectory);
			File.AppendAllText(logfile,string.Format("{1}{0}{2} ({3}){0}{4}{0}", 
				Environment.NewLine, 
				_SEPARATOR, 
				ConfigurationManager.AppSettings.Get("City"),
				DateTime.Now.ToShortTimeString(),
				string.Join(Environment.NewLine,coordinates)));
			Console.WriteLine("The calculation is complete.\nPress any key to continue.");
			Console.ReadKey();
		}
	}
	class Sector
	{
		public double start_latitude { get; set; }
		public double start_longitude { get; set; }
		public double end_latitude { get; set; }
		public double end_longitude { get; set; }

		public string getCoordinates(Sector sector)
		{
			//return string.Format("Lat: {0} Long: {1} // Lat: {2} Long: {3}",
			return string.Format("Start {0} {1} || End {2} {3}",
				sector.start_latitude.ToString("N6"),
				sector.start_longitude.ToString("N6"),
				sector.end_latitude.ToString("N6"),
				sector.end_longitude.ToString("N6"));
		}
	}
}
