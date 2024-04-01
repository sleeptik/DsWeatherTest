import {Injectable} from '@angular/core';
import {HttpClient, HttpParams} from "@angular/common/http";
import {map} from "rxjs";
import {WeatherRecord} from "../entities/weather-record";

@Injectable({
  providedIn: 'root'
})
export class ArchiveService {
  constructor(private readonly httpClient: HttpClient) {
  }

  getArchives(date: string) {
    const params = new HttpParams().set("date", date);
    return this.httpClient.get<{ [key: string]: WeatherRecord[] }>("api/archives", {params: params})
      .pipe(map(value => {
        const map = new Map<Date, WeatherRecord[]>;

        for (let valueKey in value) {
          value[valueKey].map(value1 => value1.dateTime = new Date(value1.dateTime));
        }

        for (const valueKey in value) {
          map.set(new Date(valueKey), value[valueKey]);
        }

        return map;
      }));
  }

  uploadArchives(archives: File[]) {
    const formData = new FormData();

    for (const archive of archives) {
      formData.append(archive.name, archive);
    }

    return this.httpClient.post<unknown>("api/archives/upload", formData);
  }
}
