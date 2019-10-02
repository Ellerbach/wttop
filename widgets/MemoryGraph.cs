using System;
using Microsoft.Extensions.DependencyInjection;
using Terminal.Gui;
using wttop.Widgets.Common;
using Mono.Terminal;
using wttop.Core;

namespace wttop.Widgets {

    /// <summary>
    /// Widget that will display the memory (RAM) graph
    /// </summary>
    public class MemoryGraph : WidgetFrame
    { 
        Label details;
        
        Bar bar;
        
        ISystemInfo systemInfo;

        Settings settings;

        protected override int RefreshTimeSeconds
        {
            get
            {
                return 20;
            }
        }

        public MemoryGraph(IServiceProvider serviceProvider)
        {
            systemInfo = serviceProvider.GetService<ISystemInfo>();
            settings = serviceProvider.GetService<Settings>();

            this.Title = settings.MemoryWidgetTitle;
            DrawWidget();
        }

        void DrawWidget()
        {
            Label title = new Label("Memory usage: ")
            {
                X = 1,
                Y = 1
            };
            
            bar = new Bar(settings.MemoryBarColor, settings.MainBackgroundColor)
            {
                X = Pos.Right(title),
                Y = 1,
                Width = Dim.Percent(30),
                Height= Dim.Sized(1)
            };

            details = new Label(string.Empty)
            {
                X = Pos.Left(bar) + 1,
                Y = Pos.Bottom(bar)
            };
           
            Add(title);
            Add(bar);
            Add(details);
        }
        
        protected override void Update(MainLoop MainLoop)
        {
            var memoryUsage = systemInfo.GetMemoryUsage();
            bar.Update(MainLoop, memoryUsage.PercentageUsed);
            details.Text = $"{memoryUsage.AvailableGB} GB available";
        }
    }
}