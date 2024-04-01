import {Component, Input} from '@angular/core';
import {WeatherRecord} from "../../entities/weather-record";

@Component({
  selector: 'app-archive-hourly',
  templateUrl: './archive-hourly.component.html'
})
export class ArchiveHourlyComponent {
  @Input({required: true}) weather!: WeatherRecord[] | null;
}
