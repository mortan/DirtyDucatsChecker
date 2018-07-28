using System;
using Stylet;
using StyletIoC;
using DirtyDucatsChecker.Pages;

namespace DirtyDucatsChecker
{
    public class Bootstrapper : Bootstrapper<ShellViewModel>
    {
        protected override void ConfigureIoC(IStyletIoCBuilder builder)
        {
            // Configure the IoC container in here
        }

        protected override void Configure()
        {
            // Perform any other configuration before the application starts
        }
    }
}
