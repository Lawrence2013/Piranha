﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

using Piranha.Data;
using Piranha.Rest.DataContracts;

namespace Piranha.Rest
{
	/// <summary>
	/// ReST API for categories.
	/// </summary>
	[ServiceContract()]
	[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
	public class CategoryService : BaseService
	{
		/// <summary>
		/// Gets the available contracts.
		/// </summary>
		/// <returns>The categories</returns>
		[OperationContract()]
		[WebGet(UriTemplate="get", ResponseFormat=WebMessageFormat.Json)]
		public List<Category> Get() {
			List<Category> categories = new List<Category>() ;

			Models.Category.Get(new Params() { OrderBy = "category_name" }).ForEach(c => {
				var category = new Category() {
					Id = c.Id,
					Name = c.Name,
					Permalink = c.Permalink,
					Description = c.Description,
					Created = c.Created.ToString(),
					Updated = c.Updated.ToString()
				} ;
				foreach (var ext in c.GetExtensions()) {
					var internalId = App.Instance.ExtensionManager.GetInternalIdByType(ext.Type) ;

					if (ext.Body is HtmlString)
						category.Extensions.Add(new Extension() { Name = internalId, Body = ((HtmlString)ext.Body).ToHtmlString() }) ;
					else category.Extensions.Add(new Extension() { Name = internalId, Body = ext.Body }) ;
				}
				categories.Add(category) ;
			});
			return categories ;
		}

		/// <summary>
		/// Gets the available contracts as xml.
		/// </summary>
		/// <returns>The categories</returns>
		[OperationContract()]
		[WebGet(UriTemplate="get/xml", ResponseFormat=WebMessageFormat.Xml)]
		public List<Category> GetXml() {
			return Get() ;
		}
	}
}
