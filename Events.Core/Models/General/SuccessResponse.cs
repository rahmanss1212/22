
using Events.Core.Models.DPE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Events.Core.Models.General
{
    public class SuccessResponse<T> : ControllerBase
    {
        
        public long id { get; set; }
        
        public long count { get; set; }
        public T instance { get; set; }
        
        public string Message { get; set; }

        public List<T> items { get; set; }

        public int status { get; set; } = (int)ResponseCodes.SuccessCode;

        public bool isSuccessResponseObject { get; set; } = true;

        public bool isChangeAction { get; set; } = false;


        public static SuccessResponse<T> build(T it, long i = 0, List<T> list = null)
        =>  new SuccessResponse<T>(it, i, list);

        private SuccessResponse(T it , long i = 0, List<T> list = null) {
            instance = it;
            items = list;
            id = i;
        }
        
    }
}
