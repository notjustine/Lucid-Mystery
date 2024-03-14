using System.Collections.Generic;

/**
  For attack classes that generate warnings for specific tiles.
*/
public interface IWarningGenerator
{
  public List<string> GetWarningTiles();
}