using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PaJaMaCommon
{
	public class PaJaMaResults
	{
		private List<string> _warnings = new List<string>();
		private List<string> _errors = new List<string>();
		private List<Exception> _systemExceptions = new List<Exception>();

		public void Add(PaJaMaResults results)
		{
			Errors.AddRange(results.Errors);
			Warnings.AddRange(results.Warnings);
			SystemExceptions.AddRange(results.SystemExceptions);
		}

		public void Throw()
		{
			if (Failed)
				throw new Exception(this.ErrorText);
		}

		public bool Failed
		{
			get
			{
				return (_errors.Count > 0 || _systemExceptions.Count > 0);
			}
		}

		public List<string> Warnings
		{
			get { return _warnings; }
		}

		public List<string> Errors
		{
			get { return _errors; }
		}

		public List<Exception> SystemExceptions
		{
			get { return _systemExceptions; }
		}

		public string ErrorText
		{
			get
			{
				StringBuilder sb = new StringBuilder();
				foreach (string err in this.Errors) sb.AppendLine(err);
				foreach (Exception ex in this.SystemExceptions)
				{
					sb.AppendLine(ex.Message);
					Exception innerEx = ex.InnerException;
					while (innerEx != null)
					{
						sb.AppendLine("\t" + innerEx.Message);
						innerEx = innerEx.InnerException;
					}
				}
				return sb.ToString();
			}
		}

	}


}