﻿using MediatR;
using Real_Estate_Dtos.DTO;

namespace RealEstateApi.Queries
{
    public class GetAllLocationsQuery : IRequest<List<LocationsGetAllDto>>
    {
        public int pageNumber { get; }
        public int pageSize { get; }
        public string lang { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="PageNumber"></param>
        /// <param name="PageSize"></param>
        /// <param name="Lang"></param>
        public GetAllLocationsQuery(int PageNumber, int PageSize, string Lang)
        {
            pageNumber = PageNumber;
            pageSize = PageSize;
            lang = Lang;
        }
    }
}
