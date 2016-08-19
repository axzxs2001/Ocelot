using System;
using System.Collections.Generic;
using System.Linq;
using Ocelot.Library.Infrastructure.Responses;

namespace Ocelot.Library.Infrastructure.UrlPathTemplateRepository
{
    public class InMemoryUrlPathTemplateMapRepository : IUrlPathTemplateMapRepository
    { 
        private readonly Dictionary<string, string> _routes;
        public InMemoryUrlPathTemplateMapRepository()
        {
            _routes = new Dictionary<string,string>();
        }

        public Response<List<UrlPathTemplateMap>> All
        {
            get
            {
                var routes =  _routes
                .Select(r => new UrlPathTemplateMap(r.Key, r.Value))
                .ToList();
                return new OkResponse<List<UrlPathTemplateMap>>(routes);
            }
        }

        public Response AddUrlPathTemplateMap(UrlPathTemplateMap urlPathMap)
        {
            if(_routes.ContainsKey(urlPathMap.DownstreamUrlPathTemplate))
            {
                return new ErrorResponse(new List<Error>(){new DownstreamUrlPathTemplateAlreadyExists()});
            }

            _routes.Add(urlPathMap.DownstreamUrlPathTemplate, urlPathMap.UpstreamUrlPathTemplate);

            return new OkResponse();
        }

        public Response<UrlPathTemplateMap> GetUrlPathTemplateMap(string downstreamUrlPathTemplate)
        {
            string upstreamUrlPathTemplate = null;

            if(_routes.TryGetValue(downstreamUrlPathTemplate, out upstreamUrlPathTemplate))
            {
                return new OkResponse<UrlPathTemplateMap>(new UrlPathTemplateMap(downstreamUrlPathTemplate, upstreamUrlPathTemplate));
            }

            return new ErrorResponse<UrlPathTemplateMap>(new List<Error>(){new DownstreamUrlPathTemplateDoesNotExist()});
        } 
    } 
}