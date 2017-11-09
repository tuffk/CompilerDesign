/*
  Jaime Margolin A01019332
  Juan carlos Leon A01020200
  Rodrigo Solana A01129839
*/

using System;
using System.Text;
using System.Collections.Generic;

namespace Int64 {

  public class Modishness {
    string scope;
    int? args = null;
    string name;
    Modishness(string sc, int? ar, string na)
    {
      scope = sc;
      if (ar != null)
      {
        args = ar;
      }
      name = na
    }
  }

    public class SymbolTable: IEnumerable<KeyValuePair<string, Modishness>> {

        IDictionary<string, Modishness> data = new SortedDictionary<string, Modishness>();

        //-----------------------------------------------------------
        public override string ToString() {
            var sb = new StringBuilder();
            sb.Append("Symbol Table\n");
            sb.Append("====================\n");
            foreach (var entry in data) {
                sb.Append(String.Format("{0}: {1}\n",
                                        entry.Key,
                                        entry.Value.name));
            }
            sb.Append("====================\n");
            return sb.ToString();
        }

        //-----------------------------------------------------------
        public Type this[string key] {
            get {
                return data[key];
            }
            set {
                data[key] = value;
            }
        }

        //-----------------------------------------------------------
        public bool Contains(string key) {
            return data.ContainsKey(key);
        }

        //-----------------------------------------------------------
        public IEnumerator<KeyValuePair<string, Type>> GetEnumerator() {
            return data.GetEnumerator();
        }

        //-----------------------------------------------------------
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            throw new NotImplementedException();
        }
    }
}
