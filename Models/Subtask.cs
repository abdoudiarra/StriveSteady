using System;
using System.ComponentModel.DataAnnotations;

namespace StriveSteady.Models
{
	public class Subtask
	{

		[Key]
		public int Id { get; set; }
		public string Name { get; set; }
		public string Action { get; set; }
		public Boolean IsChecked { get; set; }
	}
}

