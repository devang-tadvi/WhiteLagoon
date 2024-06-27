using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace WhiteLagoon.Domain.Entities
{
	public class Amenity
	{
		public int Id { get; set; }

		[ForeignKey("Villa")]
		public int VillaId { get; set; }

		[Required]
		public string Name { get; set; }

		public string? Description { get; set; }

		[ValidateNever]
        public Villa Villa { get; set; }
    }
}
