using Microsoft.Toolkit.Uwp.Notifications;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.System.Inventory;
using Windows.System.UserProfile;
using Windows.UI.Xaml.Controls;
namespace A5_Automation_Tool
{
    internal class Program
    {
        //Helping Function
        //Method to make size unit vary depending on the size value
        static string size_in_unit(double size)
        {
            double kilo = 1024;
            double mega = 1024 * 1024;
            double giga = 1024 * 1024 * 1024;
            double tera = giga * 1024;

            var size_unit = new StringBuilder();
            if (size > tera)
            {
                size /= tera;
                size_unit = new StringBuilder(size.ToString("F2") + " TB");
            }
            else if (size > giga)
            {
                size /= giga;
                size_unit = new StringBuilder(size.ToString("F2") + " GB");
            }

            else if (size > mega)
            {
                size /= mega;
                size_unit = new StringBuilder(size.ToString("F2") + " MB");
            }
            else if (size > kilo)
            {
                size /= kilo;
                size_unit = new StringBuilder(size.ToString("F2") + " KB");
            }
            else
            {
                size_unit = new StringBuilder(size.ToString("F2") + " Byte");
            }
            return size_unit.ToString();
        }
        static void Main(string[] args)
        {
            #region Get user name
            var user_name = Environment.UserName;
            var temp_folder_path = $@"C:\Users\{user_name}\AppData\Local\Temp";
            #endregion

            #region Check if the folder is exist
            if (!Directory.Exists(temp_folder_path))
            {
                Console.WriteLine($"Folder not found");
            }
            else
            {
                var deleted_size = 0.0;
                var size_unit = new StringBuilder();

                #region Delete files and get deleted size
                foreach (var file_name in Directory.GetFiles(temp_folder_path, "*", SearchOption.AllDirectories))
                {
                    var file = new FileInfo(file_name);
                    
                    try
                    {
                        var file_size = file.Length;
                        File.Delete(file_name);
                        deleted_size += file_size;
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"File: {file_name} >> isn't deleted");
                    }

                }
                #endregion

                #region Delete folders
                foreach (var folder in Directory.GetDirectories(temp_folder_path))
                {
                    try
                    {
                        Directory.Delete(folder, true);
                    }
                    catch (Exception)
                    {
                        Console.WriteLine($"Folder: {folder} >> isn't deleted");
                    }
                }
                #endregion

                #region Get free size in drive C
                var c_path = @"C:\";
                var c_drive = new DriveInfo(c_path);
                var c_free_size = c_drive.TotalFreeSpace;
                #endregion

                #region Show notification
                ToastContentBuilder notification = new ToastContentBuilder();
                //Notification title
                notification.AddText("Notification of cleaning the temp folder");
                //Notification message content
                notification.AddText($"Size of the deleted files: {size_in_unit(deleted_size)}\nFree space of the C: drive: {size_in_unit(c_free_size)}");
                notification.AddText(DateTime.Now.ToShortDateString());
                //لازم تحط الصورة فى المسار بتاع البرنامج bin\debug
                notification.AddInlineImage(new Uri(Path.GetFullPath(@".\KAITECH Logo.png")));
                notification.Show();
                #endregion
            }
            #endregion

        }
    }
}
