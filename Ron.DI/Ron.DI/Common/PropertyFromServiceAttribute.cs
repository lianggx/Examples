using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
public class PropertyFromServiceAttribute : Attribute, IBindingSourceMetadata
{
    public BindingSource BindingSource => BindingSource.Services;
}