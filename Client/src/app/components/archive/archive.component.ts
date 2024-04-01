import {Component, OnDestroy, OnInit} from '@angular/core';
import {WeatherRecord} from "../../entities/weather-record";
import {BehaviorSubject, Subscription, switchMap} from "rxjs";
import {FormBuilder, FormControl} from "@angular/forms";
import {ArchiveService} from "../../services/archive.service";
import {KeyValue} from "@angular/common";

@Component({
  selector: 'app-archives',
  templateUrl: './archive.component.html',
  styleUrls: ['./archive.component.css']
})
export class ArchiveComponent implements OnInit, OnDestroy {
  readonly selected: BehaviorSubject<WeatherRecord[] | null> = new BehaviorSubject<WeatherRecord[] | null>(null);
  readonly records: BehaviorSubject<Map<Date, WeatherRecord[]>> = new BehaviorSubject<Map<Date, WeatherRecord[]>>(new Map<Date, WeatherRecord[]>());
  readonly month: FormControl<string>;
  private updateSubscription!: Subscription;

  constructor(
    formBuilder: FormBuilder,
    private readonly archiveService: ArchiveService
  ) {
    this.month = formBuilder.control("2012-01", {nonNullable: true});
  }

  get firstDayOffset() {
    let date = this.records.value.keys().next().value as Date | null;

    if (!date)
      return 0;

    return (date.getDay() + 6) % 7;
  }

  ngOnDestroy(): void {
    this.updateSubscription.unsubscribe();
  }

  ngOnInit(): void {
    this.updateSubscription = this.month.valueChanges
      .pipe(switchMap(value => this.archiveService.getArchives(value)))
      .subscribe(value => {
        this.selected.next(null);
        this.records.next(value);
      });
    this.month.reset();
  }

  keyAscOrder = (a: KeyValue<Date, WeatherRecord[]>, b: KeyValue<Date, WeatherRecord[]>): number => {
    return a.key > b.key ? 1 : (b.key > a.key ? -1 : 0);
  };

  selectDay(date: Date) {
    this.selected.next(this.records.value.get(date) ?? null);
  }
}
