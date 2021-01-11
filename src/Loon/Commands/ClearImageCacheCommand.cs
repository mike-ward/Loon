using System;
using Loon.Services;

namespace Loon.Commands
{
    public class ClearImageCacheCommand : BaseCommand
    {
        public override void Execute(object? parameter)
        {
            ImageService.ClearImageCache();
            Console.Beep();
        }
    }
}