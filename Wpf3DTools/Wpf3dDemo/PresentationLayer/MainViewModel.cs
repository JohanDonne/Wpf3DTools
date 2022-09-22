using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wpf3dDemo.Domain;

namespace Wpf3dDemo.PresentationLayer;
public class MainViewModel
{
	private readonly ILogic _logic;
	public MainViewModel(ILogic logic)
	{
		_logic = logic;
	}
}
