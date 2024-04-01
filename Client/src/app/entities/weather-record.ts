import {WindDirection} from "./wind-direction";
import {WeatherActivity} from "./weather-activity";

export interface WeatherRecord {
  dateTime: Date;

  temperature: number;
  humidity: number;
  dewPoint: number;

  atmospherePressure: number;

  windSpeed: number | null;
  windDirections: WindDirection[] | null;

  overcast: number | null;
  cloudBase: number | null;
  horizontalVisibility: number | null;

  weatherActivityId: number | null;
  weatherActivity: WeatherActivity | null;
}

