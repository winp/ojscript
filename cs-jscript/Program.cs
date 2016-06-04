﻿using System;
using System.IO;
using System.Diagnostics;
using System.Security.Cryptography;

namespace orez.jscript {
	class Program {
		static void Main(string[] args) {
			var jsc = GetJscPath();

		}

		private static void Run(string bin, string args) {
			var i = new ProcessStartInfo(bin, args);
			i.CreateNoWindow = true;
			var p = Process.Start(i);
			p.WaitForExit();
		}

		/// <summary>
		/// Run Jscript.NET compiler.
		/// </summary>
		/// <param name="jsc">Jscript.NET compiler path.</param>
		/// <param name="inp">Input script file path.</param>
		/// <param name="outp">Output binary file path.</param>
		private static void Compile(string jsc, string inp, string outp) {
			var i = new ProcessStartInfo(jsc, "/nologo /out:\""+outp+"\" \""+inp+"\"");
			i.CreateNoWindow = true;
			var p = Process.Start(i);
			p.WaitForExit();
		}

		/// <summary>
		/// Return default output path for binary, and make sure it exists.
		/// </summary>
		/// <param name="inp">Input file path.</param>
		/// <returns>Full path of output binary file.</returns>
		private static string GetOutPath(string inp) {
			// prepare
			var aname = "MD5";
			var jtmp = evar("temp") + "\\0rez\\cs-jscript\\";
			// create out directory
			Directory.CreateDirectory(jtmp);
			// get hash value
			HashAlgorithm algo = HashAlgorithm.Create(aname);
			var hash = BitConverter.ToString(algo.ComputeHash(File.OpenRead(inp)));
			hash = hash.Replace("-", "").ToLower();
			// return full path
      return jtmp+hash+".exe";
		}

		/// <summary>
		/// Get Jscript.NET Compiler path.
		/// </summary>
		/// <returns>Full path of the compiler.</returns>
		private static string GetJscPath() {
			// check env var
			var jsc = Environment.GetEnvironmentVariable("JSC_PATH");
			if(jsc == null) {
				// prepare
				var jptn = "*jsc.exe";
				var jpth = evar("systemroot") + "\\microsoft.net\\framework\\";
				// srch jscript.net cmplr
				foreach(var a in Directory.GetFiles(jpth, jptn, SearchOption.AllDirectories))
					jsc = a;
				// add env var
				evar("JSC_PATH", jsc);
			}
			return jsc;
		}

		/// <summary>
		/// Get input options for ojscript.
		/// </summary>
		/// <param name="args">Input arguments.</param>
		/// <returns>Input options.</returns>
		private static oParams GetOpt(string[] args) {
			oParams p = new oParams();
			for(var i=0; i<args.Length; i++) {
				switch(args[i]) {
					case "--compile":
					case "-c":
						p.compile = true;
						break;
					case "--output":
					case "-o":
						p.output = args[++i];
						break;
					default:
						p.input = args[i];
						break;
				}
			}
			return p;
		}

		/// <summary>
		/// Shortcut to get environment variable.
		/// </summary>
		/// <param name="name">It's name.</param>
		/// <returns>It's value,</returns>
		private static string evar(string name) {
			return Environment.GetEnvironmentVariable(name);
		}
		/// <summary>
		/// Shortcut to set environment variable.
		/// </summary>
		/// <param name="name">It's name.</param>
		/// <param name="value">Tt's value.</param>
		private static void evar(string name, string value) {
			Environment.SetEnvironmentVariable(name, value);
		}
	}
}
