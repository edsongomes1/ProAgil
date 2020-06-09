import { Evento } from './../_models/Evento';
import { EventoService } from './../_services/evento.service';
import { Component, OnInit, TemplateRef } from '@angular/core';
import {  BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { FormGroup,  Validators, FormBuilder } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import {defineLocale, ptBrLocale} from 'ngx-bootstrap/chronos';

defineLocale('pt-br', ptBrLocale);


@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {
  eventosFiltrados: Evento[];
  eventos: Evento[];
  evento: Evento;
  modoSalvar = 'post';
  imagemLargura  = 50;
  imagemMargem  = 2;
  mostrarImagem = false ;
  registerForm: FormGroup;
  bodyDeletarEvento = '';
  filtroListaTemp = '';

  constructor(
               private eventoService: EventoService
             , private modalService: BsModalService
             , private fb: FormBuilder
             , private localeService: BsLocaleService
             ) {
               this.localeService.use('pt-br');
              }

  get filtroLista(): string {
    return  this.filtroListaTemp;
  }
  set filtroLista(value: string) {
    this.filtroListaTemp = value;
    this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  editarEvento(evento: Evento, template: any) {
    this.modoSalvar = 'put';
    this.openModal(template);
    this.evento = evento;
    this.registerForm.patchValue(evento);
  }
  novoEvento(template: any) {
    this.modoSalvar = 'post';
    this.openModal(template);

  }
  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.id}`;
  }

  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
        }, error => {
          console.log(error);
        }
    );
  }

  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }


  ngOnInit() {
    this.validation();
    this.getEventos();
  }

  filtrarEventos(filtrarPor: string): Evento[] {
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter( evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1);
  }

  alterarImagem() {
    this.mostrarImagem = !this.mostrarImagem;
  }
  validation() {
    // this.registerForm = new FormGroup({
    //   tema : new FormControl('',
    //   [Validators.required, Validators.minLength(4), Validators.maxLength(50)]),
    //   local : new FormControl('', Validators.required),
    //   dataEvento : new FormControl('', Validators.required),
    //   imagemURL: new FormControl('', Validators.required),
    //   qtdPessoas : new FormControl('', [Validators.required, Validators.max(120000)]),
    //   telefone : new FormControl('', Validators.required),
    //   email : new FormControl('', [Validators.required, Validators.email])
    // });
    this.registerForm = this.fb.group({
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      imagemURL: ['', Validators.required ],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]]
    });


  }
  salvarAlteracao(template: any) {
    if (this.registerForm.valid) {
      if (this.modoSalvar === 'post') {
        this.evento = Object.assign({}, this.registerForm.value);
        this.eventoService.postEvento(this.evento).subscribe(
          (novoEvento: Evento) => {
            console.log(novoEvento);
            template.hide();
            this.getEventos();
          }, error => {
            console.log(error);
          }
        );

      } else {
        this.evento = Object.assign({id: this.evento.id}, this.registerForm.value);
        this.eventoService.putEvento(this.evento).subscribe(
          () => {
            template.hide();
            this.getEventos();
          }, error => {
            console.log(error);
          }
        );
      }

    }
  }


  getEventos() {
    this.eventoService.getAllEvento().subscribe(
      (retornoEvento: Evento[]) => {
       this.eventos = retornoEvento;
       this.eventosFiltrados = this.eventos;
       console.log(retornoEvento);
      }, error => {
        console.log(error);
      });

  }

}
