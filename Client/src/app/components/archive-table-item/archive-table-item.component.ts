import {Component, Input} from '@angular/core';
import {WeatherRecord} from "../../entities/weather-record";

@Component({
  selector: 'app-archive-table-item',
  templateUrl: './archive-table-item.component.html'
})
export class ArchiveTableItemComponent {
  @Input({required: true}) weather!: WeatherRecord[];
  private readonly localizedWeekDays = ['Вс', 'Пн', 'Вт', 'Ср', 'Чт', 'Пт', 'Сб'];

  get dayName() {
    return this.localizedWeekDays[this.weather[0].dateTime.getDay()];
  }

  get day() {
    return this.weather[0].dateTime.getDate();
  }

  get maxTemp() {
    return Math.max(...this.weather.map(value => value.temperature));
  }

  get minTemp() {
    return Math.min(...this.weather.map(value => value.temperature));
  }

  get avgHumidity() {
    return this.weather.reduce((a, b) => a + b.humidity, 0) / this.weather.length;
  }

  get avgPressure() {
    return this.weather.reduce((a, b) => a + b.atmospherePressure, 0) / this.weather.length;
  }
}

