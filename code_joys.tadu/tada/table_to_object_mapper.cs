﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;

namespace tada 
{
// map table to struct
// first attempt match domain member to column name else match based on table mappings
// if object field does not have a matching column name then throw an exception
public class table_to_struct_mapper : i_table_to_object_mapper 
{
  List<table_mapping> _table_mappings;

  public table_to_struct_mapper(List<table_mapping> table_mappings) {
    _table_mappings = table_mappings;
  }

  public List<table_mapping> table_mappings { get { return _table_mappings; } }

  public List<t> map<t>(DataTable table) {
    var items = new List<t>();
    var fields = typeof(t).GetFields(BindingFlags.Public | BindingFlags.Instance);
    foreach (DataRow row in table.Rows) {
      var item = default(t);
      //if (item == null)
      //  item = (t)typeof(t).GetConstructor(null).Invoke(null);
      foreach (var field in fields) {
        if (table.Columns.Contains(field.Name))
          field.SetValueDirect(__makeref(item), row[field.Name]);
        else {
          var table_mapping = table_mappings.First(m => m.type == typeof(t));
          field.SetValueDirect(__makeref(item), row[table_mapping.get_column_name(field.Name)]);
        }
      }
      items.Add(item);
    }
    return items;
  }
}

public interface i_table_to_object_mapper {
  List<t> map<t>(DataTable table);
  List<table_mapping> table_mappings { get; }
}

public class column_mapping
{
   public string domain_member, column_name;
}

public class table_mapping
{
   public string table; // optional to shorten sql
   public Type type; // required (depends on table to object mapper implementation
   public List<column_mapping> column_mappings = new List<column_mapping>();

   // required to map fields whose name does not match a column name
   protected void map(string domain_member, string column_name) {
      column_mappings.Add(new column_mapping() { domain_member = domain_member, column_name = column_name });
   }

   public string get_column_name(string domain_member) {
      return column_mappings.First(m => m.domain_member == domain_member).column_name;
   }
}
}