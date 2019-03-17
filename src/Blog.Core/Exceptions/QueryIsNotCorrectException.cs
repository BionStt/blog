using System;
using System.Collections.Generic;

namespace Blog.Core.Exceptions
{
    public class QueryIsNotCorrectException : Exception
    {
        public Dictionary<String, List<String>> QueryErrors;

        public QueryIsNotCorrectException(String fieldName)
        {
            QueryErrors= new Dictionary<String, List<String>>
            {
                {fieldName, new List<String>{"Field is required or default value is not acceptable."}}
            };
        }

        public QueryIsNotCorrectException(String fieldName,
                                          String errorMessage)
        {
            QueryErrors = new Dictionary<String, List<String>>
            {
                {fieldName, new List<String> {errorMessage}}
            };
        }

        public QueryIsNotCorrectException(Dictionary<String, List<String>> errors)
        {
            QueryErrors = errors;
        }

        public QueryIsNotCorrectException(String fieldName,
                                          List<String> errors)
        {
            QueryErrors = new Dictionary<String, List<String>> {{fieldName, errors}};
        }
    }
}