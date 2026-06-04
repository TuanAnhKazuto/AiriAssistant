using System;
using System.IO;
using System.Media;
using System.Windows;

namespace AiriAssistant.Services
{
    public class SoundService
    {
        public void Play(string relativePath)
        {
            string fullPath = System.IO.Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                relativePath
            );

            if (!System.IO.File.Exists(fullPath))
            {
                System.Windows.MessageBox.Show($"Không tìm thấy file sound:\n{fullPath}");
                return;
            }

            SoundPlayer player = new(fullPath);
            player.Play();
        }
    }
}
