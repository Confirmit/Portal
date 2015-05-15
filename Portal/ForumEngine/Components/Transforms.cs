using System;
using System.Collections;
using System.Collections.Specialized;
using System.Web;
using System.IO;
using System.Web.Caching;
using System.Text.RegularExpressions;
using System.Text;

namespace AspNetForums.Components {

    public class Transforms {

        // *********************************************************************
        //  TranformString
        //
        /// <summary>
        /// Method used to transform contents of string based on settings of forums
        /// </summary>
        /// 
        // ********************************************************************/
        public static string TransformString(string stringToTransform) {
            string transformedString;
            ArrayList userDefinedTransforms;

            // Load the transform table
            userDefinedTransforms = LoadUserDefinedTransforms();

            // Html Encode the contents
            stringToTransform = HttpContext.Current.Server.HtmlEncode(stringToTransform);

            // Perform user defined transforms
            transformedString = PerformUserTransforms(stringToTransform, userDefinedTransforms);

            // Ensure we have safe anchors
            transformedString = EnsureSafeAnchors(transformedString);

            // Return the new string
            return transformedString.Replace("\n", "\n" + Globals.HtmlNewLine + "\n");
        }


        private static string EnsureSafeAnchors(string stringToTransform) {
            MatchCollection matchs;
                        
            // Ensure we have safe anchors
            matchs = Regex.Matches(stringToTransform, "&lt;a.href=&quot;(?<url>http://((.|\\n)*?))&quot;&gt;(?<target>((.|\\n)*?))&lt;/a&gt;", RegexOptions.IgnoreCase | RegexOptions.Compiled);

            foreach (Match m in matchs) {
                stringToTransform = stringToTransform.Replace(m.ToString(), "<a target=\"_new\" href=\"" + m.Groups["url"].ToString() + "\">" + m.Groups["target"].ToString() + "</a>");
            }

            return stringToTransform;
        }


        // *********************************************************************
        //  PerformUserTransforms
        //
        /// <summary>
        /// Performs the user defined transforms
        /// </summary>
        /// 
        // ********************************************************************/
        private static string PerformUserTransforms(string stringToTransform, ArrayList userDefinedTransforms) {
            int iLoop = 0;			

            while (iLoop < userDefinedTransforms.Count) {		
		        
                // Special work for anchors
                stringToTransform = Regex.Replace(stringToTransform, userDefinedTransforms[iLoop].ToString(), userDefinedTransforms[iLoop+1].ToString(), RegexOptions.IgnoreCase | RegexOptions.Compiled | RegexOptions.Multiline);

                iLoop += 2;
            }

            return stringToTransform;
        }


        // *********************************************************************
        //  LoadUserDefinedTransforms
        //
        /// <summary>
        /// Returns a array list containing transforms that the user defined. Usually
        /// in transforms.txt
        /// </summary>
        /// 
        // ********************************************************************/
        private static ArrayList LoadUserDefinedTransforms() {
            ArrayList tranforms;
            string filenameOfTransformFile;

            // read the transformation hashtable from the cache
            tranforms = (ArrayList) HttpContext.Current.Cache.Get("transformTable");
            if (tranforms == null) {
                tranforms = new ArrayList();

                // build up the hashtable and store it in the cache
                // start by opening the text file
                filenameOfTransformFile = Globals.PhysicalPathToTransformationFile;
                if (filenameOfTransformFile.Length > 0) {
                    StreamReader sr = File.OpenText(filenameOfTransformFile);

                    // now, read through each set of lines in the text file
                    string line = sr.ReadLine(); 
                    string replaceLine = "";

                    while (line != null) {
                        line = Regex.Escape(line);
                        replaceLine = sr.ReadLine();

                        // make sure replaceLine != null
                        if (replaceLine == null) 
                            break;
					
                        line = line.Replace("<CONTENTS>", "((.|\n)*?)");
                        line = line.Replace("<WORDBOUNDARY>", "\\b");
                        line = line.Replace("<", "&lt;");
                        line = line.Replace(">", "&gt;");
                        line = line.Replace("\"", "&quot;");

                        replaceLine = replaceLine.Replace("<CONTENTS>", "$1");					
					
                        tranforms.Add(line);
                        tranforms.Add(replaceLine);

                        line = sr.ReadLine();

                    }

                    // close the streamreader
                    sr.Close();		

                    // slap the ArrayList into the cache and set its dependency to the transform file.
                    HttpContext.Current.Cache.Insert("transformTable", tranforms, new CacheDependency(filenameOfTransformFile));
                }
            }
  
            return (ArrayList) HttpContext.Current.Cache["transformTable"];
        }
    }
}