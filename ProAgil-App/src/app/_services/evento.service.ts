import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../_models/Evento';

//quando passamos esse decorate que tem esse provideIn root diz que pode ser injetado em qualquer lugar, por isso que conseguimos usar no componente de evento
//caso não tivivesse isso teriamos que ir no componente que utilizamos dentro do objeto @Component e adicionamos a tag providers: 
//ou caso nao queira nenhum dos dois, temos que polocar no app.modules.ts na parte de provide
@Injectable({
  providedIn: 'root'
})
export class EventoService {

    baseURL = 'http://localhost:5000/api/evento';
    constructor(private http: HttpClient) { }

    getAllEvento(): Observable<Evento[]> {

      //aqui retorna um observable.
        return this.http.get<Evento[]>(this.baseURL);
    }

    getEventoByTema(tema: string): Observable<Evento[]> {

      //aqui retorna um observable.
      //utilizando template string
      return this.http.get<Evento[]>(`${this.baseURL}/getByTema/${tema}`);
    }

    getEventoById(id: number): Observable<Evento> {

      //aqui retorna um observable.
        return this.http.get<Evento>(`${this.baseURL}/${id}`);
    }
}
