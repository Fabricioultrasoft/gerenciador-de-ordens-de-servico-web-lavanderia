-- cria o banco de dados ja nao existir
CREATE DATABASE IF NOT EXISTS gabbeh_db CHARACTER SET latin1 COLLATE latin1_swedish_ci;
-- seta o banco de dados corrente
USE gabbeh_db;

-- cria usuario e atribui as permissoes necessarias
CREATE USER 'simple_user'@'localhost' IDENTIFIED BY '12345';
GRANT SELECT,INSERT,UPDATE,DELETE ON gabbeh_db.* TO 'simple_user'@'localhost';
FLUSH PRIVILEGES;

-- cria as tabelas da aplicacao
CREATE TABLE tb_tapetes (
  cod_tapete INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_tapete VARCHAR(100) NOT NULL,
  txt_descricao TEXT NULL,
  flg_ativo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY(cod_tapete)
)
ENGINE=InnoDB;

INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão com Seda e Chinile');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão com Chinile');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Sisal com Algodão');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Sisal Comum');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão com Seda e Chinile (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Algodão com Chinile (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Sisal com Algodão (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Sisal Comum (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Industrializado (Pelo Baixo)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Belga (GROSSO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Industrializado Importado (Egipcio, Turco, Francês, etc)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Industrializado (PELO ALTO)');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Arraiolo');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Belga Comum');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Kilim Colorido');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Kilim Persa');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Pele de Carneiro');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapeçaria');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapeçaria Antigo');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapete com Seda');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapete Santa Helena');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapetes Antigos');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapetes Orientais');
INSERT INTO tb_tapetes(nom_tapete) VALUES('Tapetes Orientais Gabbeh e Chines');


CREATE TABLE tb_tipos_clientes (
  cod_tipo_cliente INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_tipo_cliente VARCHAR(100) NOT NULL,
  flg_ativo BIT NOT NULL DEFAULT 1,
  PRIMARY KEY(cod_tipo_cliente)
)
ENGINE=InnoDB;

INSERT INTO tb_tipos_clientes(nom_tipo_cliente) VALUES('Cliente comum');
INSERT INTO tb_tipos_clientes(nom_tipo_cliente) VALUES('Lavanderia');
INSERT INTO tb_tipos_clientes(nom_tipo_cliente) VALUES('Decoradora'); 
 


CREATE TABLE tb_tipos_logradouros (
  cod_tipo_logradouro INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_tipo_logradouro VARCHAR(25) NOT NULL,
  PRIMARY KEY(cod_tipo_logradouro)
)
ENGINE=InnoDB;

CREATE TABLE tb_servicos (
  cod_servico INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_servico VARCHAR(255) NOT NULL,
  int_cobrado_por INTEGER UNSIGNED NOT NULL,
  txt_descricao TEXT NULL,
  PRIMARY KEY(cod_servico)
)
ENGINE=InnoDB;

ALTER TABLE tb_servicos AUTO_INCREMENT = 1; 
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Lavagem',2);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Franja de algodão',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Franja de lã',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Franja de seda',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Amarrar franja',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Cordão (feito á mão)',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Debrum (feito á maquina com fita)',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Debrum (feito á maquina)',1);
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Cola',2); 
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Forro de algodão',2); 
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Couro',2); 
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Goma em Sisal',1); 
INSERT INTO tb_servicos(nom_servico, int_cobrado_por) VALUES('Antiderrapante',2); 


CREATE TABLE tb_paises (
  cod_pais INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_pais VARCHAR(25) NOT NULL,
  PRIMARY KEY(cod_pais)
)
ENGINE=InnoDB;

insert into tb_paises(nom_pais)values('Brasil');


CREATE TABLE tb_status_os (
  cod_status_os INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_status_os VARCHAR(50) NOT NULL,
  PRIMARY KEY(cod_status_os)
)
ENGINE=InnoDB;

INSERT INTO tb_status_os (nom_status_os) VALUES ('Aberto');
INSERT INTO tb_status_os (nom_status_os) VALUES ('Finalizado');
INSERT INTO tb_status_os (nom_status_os) VALUES ('Cancelado');


CREATE TABLE tb_usuarios (
  cod_usuario INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_usuario VARCHAR(100) NOT NULL,
  txt_senha VARCHAR(40) NOT NULL,
  PRIMARY KEY(cod_usuario)
)
ENGINE=InnoDB;

ALTER TABLE tb_usuarios AUTO_INCREMENT = 1; 
INSERT INTO tb_usuarios(nom_usuario,txt_senha) VALUES ('admin',SHA1('admin'));


CREATE TABLE tb_tipos_meios_de_contato (
  cod_tipo_meio_de_contato INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_tipo_meio_de_contato VARCHAR(50) NOT NULL,
  PRIMARY KEY(cod_tipo_meio_de_contato)
)
ENGINE=InnoDB;

INSERT INTO tb_tipos_meios_de_contato(nom_tipo_meio_de_contato) VALUES ('Tel. Residencial');
INSERT INTO tb_tipos_meios_de_contato(nom_tipo_meio_de_contato) VALUES ('Tel. Comercial');
INSERT INTO tb_tipos_meios_de_contato(nom_tipo_meio_de_contato) VALUES ('Celular');
INSERT INTO tb_tipos_meios_de_contato(nom_tipo_meio_de_contato) VALUES ('Radio');
INSERT INTO tb_tipos_meios_de_contato(nom_tipo_meio_de_contato) VALUES ('Outros');


CREATE TABLE tb_estados (
  cod_estado INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_estado VARCHAR(25) NOT NULL,
  cod_pais INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY(cod_estado),
  INDEX ix_cod_pais(cod_pais),
  FOREIGN KEY(cod_pais)
    REFERENCES tb_paises(cod_pais)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Acre');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Alagoas');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Amapá');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Amazonas');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Bahia');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Ceará');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Distrito Federal');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Espírito Santo');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Goiás');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Maranhão');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Mato Grosso');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Mato Grosso do Sul');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Minas Gerais');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Pará');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Paraíba');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Paraná');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Pernambuco');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Piauí');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Rio de Janeiro');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Rio Grande do Norte');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Rio Grande do Sul');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Rondônia');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Roraima');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Santa Catarina');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'São Paulo');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Sergipe');  
INSERT INTO tb_estados (cod_pais, nom_estado) VALUES (1, 'Tocantins');


CREATE TABLE tb_clientes (
  cod_cliente INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_tipo_cliente INTEGER UNSIGNED NOT NULL,
  nom_cliente VARCHAR(100) NOT NULL,
  nom_conjuge VARCHAR(100) NULL,
  int_sexo TINYINT UNSIGNED NOT NULL DEFAULT 1,
  dat_nascimento TIMESTAMP NULL,
  txt_rg VARCHAR(12) NULL,
  txt_cpf VARCHAR(14) NULL,
  txt_observacoes TEXT NULL,
  flg_ativo BIT NOT NULL DEFAULT 1,
  dat_cadastro TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ,
  dat_atualizacao TIMESTAMP NOT NULL DEFAULT '2001-01-01 00:00:00',
  PRIMARY KEY(cod_cliente),
  INDEX ix_cod_tipo_cliente(cod_tipo_cliente),
  INDEX ix_nom_cliente(nom_cliente),
  FOREIGN KEY(cod_tipo_cliente)
    REFERENCES tb_tipos_clientes(cod_tipo_cliente)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_meios_de_contato (
  cod_meio_de_contato INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_cliente INTEGER UNSIGNED NOT NULL,
  cod_tipo_meio_de_contato INTEGER UNSIGNED NOT NULL,
  txt_meio_de_contato VARCHAR(100) NOT NULL,
  int_meio_de_contato INTEGER UNSIGNED NULL,
  txt_descricao TEXT NULL,
  PRIMARY KEY(cod_meio_de_contato),
  INDEX ix_cod_tipo_meio_de_contato(cod_tipo_meio_de_contato),
  INDEX ix_cod_cliente(cod_cliente),
  FOREIGN KEY(cod_tipo_meio_de_contato)
    REFERENCES tb_tipos_meios_de_contato(cod_tipo_meio_de_contato)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_cliente)
    REFERENCES tb_clientes(cod_cliente)
      ON DELETE CASCADE
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_valores_servicos (
  cod_valor_servico INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_valor_servico_pai INTEGER UNSIGNED NULL,
  cod_servico INTEGER UNSIGNED NOT NULL,
  cod_tapete INTEGER UNSIGNED NOT NULL,
  cod_tipo_cliente INTEGER UNSIGNED NULL,
  val_inicial DOUBLE NOT NULL DEFAULT 0,
  val_acima_10m2 DOUBLE NOT NULL DEFAULT 0,
  PRIMARY KEY(cod_valor_servico),
  INDEX ix_cod_tapete(cod_tapete),
  INDEX ix_cod_servico(cod_servico),
  INDEX ix_cod_valor_servico_pai(cod_valor_servico_pai),
  INDEX ix_cod_tipo_cliente(cod_tipo_cliente),
  FOREIGN KEY(cod_tapete)
    REFERENCES tb_tapetes(cod_tapete)
      ON DELETE CASCADE
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_servico)
    REFERENCES tb_servicos(cod_servico)
      ON DELETE CASCADE
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_valor_servico_pai)
    REFERENCES tb_valores_servicos(cod_valor_servico)
      ON DELETE CASCADE
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_tipo_cliente)
    REFERENCES tb_tipos_clientes(cod_tipo_cliente)
      ON DELETE CASCADE
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

ALTER TABLE tb_valores_servicos AUTO_INCREMENT = 1; 
-- SERVICO - Lavagem
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,1,17.00,20.00); -- '1', 'Algodão'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,2,20.00,20.00); -- '2', 'Algodão com Seda e Chinile'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,3,20.00,20.00); -- '3', 'Algodão com Chinile'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,4,17.00,20.00); -- '4', 'Sisal com Algodão'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,5,17.00,20.00); -- '5', 'Sisal Comum'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,6,25.00,30.00); -- '6', 'Algodão (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,7,25.00,30.00); -- '7', 'Algodão com Seda e Chinile (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,8,25.00,30.00); -- '8', 'Algodão com Chinile (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,9,25.00,30.00); -- '9', 'Sisal com Algodão (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,10,20.00,30.00); -- '10', 'Sisal Comum (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,11,15.00,20.00); -- '11', 'Industrializado (Pelo Baixo)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,12,17.00,20.00); -- '12', 'Belga (GROSSO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,13,20.00,22.00); -- '13', 'Industrializado Importado (Egipcio, Turco, Francês, etc)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,14,20.00,22.00); -- '14', 'Industrializado (PELO ALTO)'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,15,17.00,20.00); -- '15', 'Arraiolo'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,16,17.00,20.00); -- '16', 'Belga Comum'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,17,17.00,20.00); -- '17', 'Kilim Colorido'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,18,25.00,30.00); -- '18', 'Kilim Persa'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,19,30.00,35.00); -- '19', 'Pele de Carneiro'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,20,30.00,40.00); -- '20', 'Tapeçaria'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,21,50.00,60.00); -- '21', 'Tapeçaria Antigo'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,22,50.00,60.00); -- '22', 'Tapete com Seda'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,23,35.00,40.00); -- '23', 'Tapete Santa Helena'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,24,50.00,60.00); -- '24', 'Tapetes Antigos'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,25,35.00,40.00); -- '25', 'Tapetes Orientais'
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (1,26,50.00,60.00); -- '26', 'Tapetes Orientais Gabbeh e Chines'
-- Lavagem para Lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 1, 1, 1, 2, 13.00, 16.00 ); -- lavagem, algodão, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 2, 1, 2, 2, 13.00, 16.00 ); -- lavagem, Algodão com Seda e Chinile, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 3, 1, 3, 2, 13.00, 16.00 ); -- lavagem, Algodão com Chinile, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 4, 1, 4, 2, 13.00, 16.00 ); -- lavagem, Sisal com Algodão, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 5, 1, 5, 2, 13.00, 16.00 ); -- lavagem, Sisal Comum, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 16, 1, 16, 2, 11.00, 13.00 ); -- lavagem, Belga Comum, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 13, 1, 13, 2, 11.00, 13.00 ); -- lavagem, Industrializado Importado (Egipcio, Turco, Francês, etc), lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 15, 1, 15, 2, 11.00, 13.00 ); -- lavagem, Arraiolo, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 17, 1, 17, 2, 11.00, 13.00 ); -- lavagem, Kilim Colorido, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 18, 1, 18, 2, 17.00, 22.00 ); -- lavagem, Kilim Persa, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 19, 1, 19, 2, 17.00, 22.00 ); -- lavagem, Pele de Carneiro, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 23, 1, 23, 2, 17.00, 22.00 ); -- lavagem, Tapete Santa Helena, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 25, 1, 25, 2, 17.00, 22.00 ); -- lavagem, Tapetes Orientais, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 26, 1, 26, 2, 20.00, 25.00 ); -- lavagem, Tapetes Orientais Gabbeh e Chines, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 20, 1, 20, 2, 30.00, 40.00 ); -- lavagem, Tapeçaria, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 21, 1, 21, 2, 30.00, 40.00 ); -- lavagem, Tapeçaria Antigo, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 22, 1, 22, 2, 30.00, 40.00 ); -- lavagem, Tapete com Seda, lavanderia
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES ( 24, 1, 24, 2, 0.00, 0.00 ); -- lavagem, Tapetes Antigos, lavanderia
-- --------------------------------------------
-- SERVICO - Franja de Algodão
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,1,60.00,60.00); -- 45
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,2,60.00,60.00); -- 46
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,3,60.00,60.00); -- 47
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,4,60.00,60.00); -- 48
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,5,60.00,60.00); -- 49
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,6,60.00,60.00); -- 50
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,7,60.00,60.00); -- 51
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,8,60.00,60.00); -- 52
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,9,60.00,60.00); -- 53
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,10,60.00,60.00); -- 54
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,11,60.00,60.00); -- 55
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,12,60.00,60.00); -- 56
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,13,60.00,60.00); -- 57
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,14,60.00,60.00); -- 58
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,15,60.00,60.00); -- 59
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,16,60.00,60.00); -- 60
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,17,60.00,60.00); -- 61
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,18,60.00,60.00); -- 62
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,19,60.00,60.00); -- 63
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,20,60.00,60.00); -- 64
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,21,60.00,60.00); -- 65
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,22,60.00,60.00); -- 66
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,23,60.00,60.00); -- 67
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,24,60.00,60.00); -- 68
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,25,60.00,60.00); -- 69
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (2,26,60.00,60.00); -- 70
-- SERVICO - Franja de Algodão - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (45,2,1,2,35.00,35.00); -- 71
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (46,2,2,2,35.00,35.00); -- 72
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (47,2,3,2,35.00,35.00); -- 73
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (48,2,4,2,35.00,35.00); -- 74
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (49,2,5,2,35.00,35.00); -- 75
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (50,2,6,2,35.00,35.00); -- 76
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (51,2,7,2,35.00,35.00); -- 77
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (52,2,8,2,35.00,35.00); -- 78
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (53,2,9,2,35.00,35.00); -- 79
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (54,2,10,2,35.00,35.00); -- 80
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (55,2,11,2,35.00,35.00); -- 81
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (56,2,12,2,35.00,35.00); -- 82
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (57,2,13,2,35.00,35.00); -- 83
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (58,2,14,2,35.00,35.00); -- 84
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (59,2,15,2,35.00,35.00); -- 85
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (60,2,16,2,35.00,35.00); -- 86
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (61,2,17,2,35.00,35.00); -- 87
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (62,2,18,2,35.00,35.00); -- 88
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (63,2,19,2,35.00,35.00); -- 89
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (64,2,20,2,35.00,35.00); -- 90
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (65,2,21,2,35.00,35.00); -- 91
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (66,2,22,2,35.00,35.00); -- 92
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (67,2,23,2,35.00,35.00); -- 93
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (68,2,24,2,35.00,35.00); -- 94
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (69,2,25,2,35.00,35.00); -- 95
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (70,2,26,2,35.00,35.00); -- 96
-- --------------------------------------------
-- SERVICO - Franja de lã
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,1,70.00,70.00); -- 97
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,2,70.00,70.00); -- 98
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,3,70.00,70.00); -- 99
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,4,70.00,70.00); -- 100
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,5,70.00,70.00); -- 101
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,6,70.00,70.00); -- 102
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,7,70.00,70.00); -- 103
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,8,70.00,70.00); -- 104
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,9,70.00,70.00); -- 105
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,10,70.00,70.00); -- 106
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,11,70.00,70.00); -- 107
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,12,70.00,70.00); -- 108
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,13,70.00,70.00); -- 109
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,14,70.00,70.00); -- 110
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,15,70.00,70.00); -- 111
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,16,70.00,70.00); -- 112
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,17,70.00,70.00); -- 113
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,18,70.00,70.00); -- 114
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,19,70.00,70.00); -- 115
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,20,70.00,70.00); -- 116
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,21,70.00,70.00); -- 117
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,22,70.00,70.00); -- 118
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,23,70.00,70.00); -- 119
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,24,70.00,70.00); -- 120
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,25,70.00,70.00); -- 121
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (3,26,70.00,70.00); -- 122
-- SERVICO - Franja de lã - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (97,3,1,2,37.00,37.00); -- 123
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (98,3,2,2,37.00,37.00); -- 124
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (99,3,3,2,37.00,37.00); -- 125
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (100,3,4,2,37.00,37.00); -- 126
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (101,3,5,2,37.00,37.00); -- 127
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (102,3,6,2,37.00,37.00); -- 128
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (103,3,7,2,37.00,37.00); -- 129
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (104,3,8,2,37.00,37.00); -- 130
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (105,3,9,2,37.00,37.00); -- 131
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (106,3,10,2,37.00,37.00); -- 132
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (107,3,11,2,37.00,37.00); -- 133
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (108,3,12,2,37.00,37.00); -- 134
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (109,3,13,2,37.00,37.00); -- 135
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (110,3,14,2,37.00,37.00); -- 136
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (111,3,15,2,37.00,37.00); -- 137
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (112,3,16,2,37.00,37.00); -- 138
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (113,3,17,2,37.00,37.00); -- 139
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (114,3,18,2,37.00,37.00); -- 140
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (115,3,19,2,37.00,37.00); -- 141
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (116,3,20,2,37.00,37.00); -- 142
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (117,3,21,2,37.00,37.00); -- 143
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (118,3,22,2,37.00,37.00); -- 144
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (119,3,23,2,37.00,37.00); -- 145
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (120,3,24,2,37.00,37.00); -- 146
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (121,3,25,2,37.00,37.00); -- 147
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (122,3,26,2,37.00,37.00); -- 148
-- --------------------------------------------
-- SERVICO - Franja de seda
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,1,120.00,120.00); -- 149
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,2,120.00,120.00); -- 150
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,3,120.00,120.00); -- 151
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,4,120.00,120.00); -- 152
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,5,120.00,120.00); -- 153
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,6,120.00,120.00); -- 154
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,7,120.00,120.00); -- 155
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,8,120.00,120.00); -- 156
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,9,120.00,120.00); -- 157
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,10,120.00,120.00); -- 158
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,11,120.00,120.00); -- 159
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,12,120.00,120.00); -- 160
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,13,120.00,120.00); -- 161
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,14,120.00,120.00); -- 162
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,15,120.00,120.00); -- 163
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,16,120.00,120.00); -- 164
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,17,120.00,120.00); -- 165
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,18,120.00,120.00); -- 166
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,19,120.00,120.00); -- 167
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,20,120.00,120.00); -- 168
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,21,120.00,120.00); -- 169
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,22,120.00,120.00); -- 170
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,23,120.00,120.00); -- 171
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,24,120.00,120.00); -- 172
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,25,120.00,120.00); -- 173
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (4,26,120.00,120.00); -- 174
-- SERVICO - Franja de seda - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (149,4,1,2,50.00,50.00); -- 175
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (150,4,2,2,50.00,50.00); -- 176
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (151,4,3,2,50.00,50.00); -- 177
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (152,4,4,2,50.00,50.00); -- 178
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (153,4,5,2,50.00,50.00); -- 179
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (154,4,6,2,50.00,50.00); -- 180
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (155,4,7,2,50.00,50.00); -- 181
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (156,4,8,2,50.00,50.00); -- 182
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (157,4,9,2,50.00,50.00); -- 183
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (158,4,10,2,50.00,50.00); -- 184
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (159,4,11,2,50.00,50.00); -- 185
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (160,4,12,2,50.00,50.00); -- 186
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (161,4,13,2,50.00,50.00); -- 187
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (162,4,14,2,50.00,50.00); -- 188
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (163,4,15,2,50.00,50.00); -- 189
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (164,4,16,2,50.00,50.00); -- 190
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (165,4,17,2,50.00,50.00); -- 191
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (166,4,18,2,50.00,50.00); -- 192
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (167,4,19,2,50.00,50.00); -- 193
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (168,4,20,2,50.00,50.00); -- 194
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (169,4,21,2,50.00,50.00); -- 195
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (170,4,22,2,50.00,50.00); -- 196
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (171,4,23,2,50.00,50.00); -- 197
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (172,4,24,2,50.00,50.00); -- 198
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (173,4,25,2,50.00,50.00); -- 199
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (174,4,26,2,50.00,50.00); -- 200
-- --------------------------------------------
-- SERVICO - Amarrar Franja
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,1,20.00,20.00); -- 201
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,2,20.00,20.00); -- 202
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,3,20.00,20.00); -- 203
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,4,20.00,20.00); -- 204
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,5,20.00,20.00); -- 205
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,6,20.00,20.00); -- 206
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,7,20.00,20.00); -- 207
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,8,20.00,20.00); -- 208
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,9,20.00,20.00); -- 209
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,10,20.00,20.00); -- 210
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,11,20.00,20.00); -- 211
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,12,20.00,20.00); -- 212
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,13,20.00,20.00); -- 213
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,14,20.00,20.00); -- 214
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,15,20.00,20.00); -- 215
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,16,20.00,20.00); -- 216
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,17,20.00,20.00); -- 217
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,18,20.00,20.00); -- 218
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,19,20.00,20.00); -- 219
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,20,20.00,20.00); -- 220
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,21,20.00,20.00); -- 221
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,22,20.00,20.00); -- 222
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,23,20.00,20.00); -- 223
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,24,20.00,20.00); -- 224
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,25,20.00,20.00); -- 225
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (5,26,20.00,20.00); -- 226
-- SERVICO - Amarrar Franja - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (201,5,1,2,15.00,15.00); -- 227
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (202,5,2,2,15.00,15.00); -- 228
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (203,5,3,2,15.00,15.00); -- 229
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (204,5,4,2,15.00,15.00); -- 230
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (205,5,5,2,15.00,15.00); -- 231
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (206,5,6,2,15.00,15.00); -- 232
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (207,5,7,2,15.00,15.00); -- 233
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (208,5,8,2,15.00,15.00); -- 234
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (209,5,9,2,15.00,15.00); -- 235
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (210,5,10,2,15.00,15.00); -- 236
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (211,5,11,2,15.00,15.00); -- 237
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (212,5,12,2,15.00,15.00); -- 238
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (213,5,13,2,15.00,15.00); -- 239
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (214,5,14,2,15.00,15.00); -- 240
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (215,5,15,2,15.00,15.00); -- 241
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (216,5,16,2,15.00,15.00); -- 242
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (217,5,17,2,15.00,15.00); -- 243
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (218,5,18,2,15.00,15.00); -- 244
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (219,5,19,2,15.00,15.00); -- 245
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (220,5,20,2,15.00,15.00); -- 246
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (221,5,21,2,15.00,15.00); -- 247
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (222,5,22,2,15.00,15.00); -- 248
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (223,5,23,2,15.00,15.00); -- 249
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (224,5,24,2,15.00,15.00); -- 250
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (225,5,25,2,15.00,15.00); -- 251
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (226,5,26,2,15.00,15.00); -- 252
-- --------------------------------------------
-- SERVICO - Cordão (feito á mão)
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,1,40.00,40.00); -- 253
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,2,40.00,40.00); -- 254
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,3,40.00,40.00); -- 255
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,4,40.00,40.00); -- 256
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,5,40.00,40.00); -- 257
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,6,40.00,40.00); -- 258
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,7,40.00,40.00); -- 259
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,8,40.00,40.00); -- 260
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,9,40.00,40.00); -- 261
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,10,40.00,40.00); -- 262
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,11,40.00,40.00); -- 263
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,12,40.00,40.00); -- 264
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,13,40.00,40.00); -- 265
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,14,40.00,40.00); -- 266
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,15,40.00,40.00); -- 267
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,16,40.00,40.00); -- 268
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,17,40.00,40.00); -- 269
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,18,40.00,40.00); -- 270
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,19,40.00,40.00); -- 271
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,20,40.00,40.00); -- 272
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,21,40.00,40.00); -- 273
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,22,40.00,40.00); -- 274
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,23,40.00,40.00); -- 275
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,24,40.00,40.00); -- 276
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,25,40.00,40.00); -- 277
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (6,26,40.00,40.00); -- 278
-- SERVICO - Cordão (feito á mão) - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (253,6,1,2,27.00,27.00); -- 279
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (254,6,2,2,27.00,27.00); -- 280
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (255,6,3,2,27.00,27.00); -- 281
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (256,6,4,2,27.00,27.00); -- 282
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (257,6,5,2,27.00,27.00); -- 283
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (258,6,6,2,27.00,27.00); -- 284
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (259,6,7,2,27.00,27.00); -- 285
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (260,6,8,2,27.00,27.00); -- 286
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (261,6,9,2,27.00,27.00); -- 287
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (262,6,10,2,27.00,27.00); -- 288
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (263,6,11,2,27.00,27.00); -- 289
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (264,6,12,2,27.00,27.00); -- 290
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (265,6,13,2,27.00,27.00); -- 291
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (266,6,14,2,27.00,27.00); -- 292
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (267,6,15,2,27.00,27.00); -- 293
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (268,6,16,2,27.00,27.00); -- 294
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (269,6,17,2,27.00,27.00); -- 295
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (270,6,18,2,27.00,27.00); -- 296
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (271,6,19,2,27.00,27.00); -- 297
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (272,6,20,2,27.00,27.00); -- 298
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (273,6,21,2,27.00,27.00); -- 299
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (274,6,22,2,27.00,27.00); -- 300
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (275,6,23,2,27.00,27.00); -- 301
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (276,6,24,2,27.00,27.00); -- 302
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (277,6,25,2,27.00,27.00); -- 303
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (278,6,26,2,27.00,27.00); -- 304
-- --------------------------------------------
-- SERVICO - Debrum (feito á maquina com fita)
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,1,20.00,20.00); -- 305
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,2,20.00,20.00); -- 306
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,3,20.00,20.00); -- 307
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,4,20.00,20.00); -- 308
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,5,20.00,20.00); -- 309
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,6,20.00,20.00); -- 310
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,7,20.00,20.00); -- 311
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,8,20.00,20.00); -- 312
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,9,20.00,20.00); -- 313
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,10,20.00,20.00); -- 314
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,11,20.00,20.00); -- 315
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,12,20.00,20.00); -- 316
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,13,20.00,20.00); -- 317
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,14,20.00,20.00); -- 318
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,15,20.00,20.00); -- 319
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,16,20.00,20.00); -- 320
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,17,20.00,20.00); -- 321
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,18,20.00,20.00); -- 322
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,19,20.00,20.00); -- 323
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,20,20.00,20.00); -- 324
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,21,20.00,20.00); -- 325
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,22,20.00,20.00); -- 326
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,23,20.00,20.00); -- 327
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,24,20.00,20.00); -- 328
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,25,20.00,20.00); -- 329
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (7,26,20.00,20.00); -- 330
-- SERVICO - Debrum (feito á maquina com fita) - LAVANDERIA
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (305,7,1,2,15.00,15.00); -- 331
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (306,7,2,2,15.00,15.00); -- 332
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (307,7,3,2,15.00,15.00); -- 333
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (308,7,4,2,15.00,15.00); -- 334
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (309,7,5,2,15.00,15.00); -- 335
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (310,7,6,2,15.00,15.00); -- 336
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (311,7,7,2,15.00,15.00); -- 337
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (312,7,8,2,15.00,15.00); -- 338
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (313,7,9,2,15.00,15.00); -- 339
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (314,7,10,2,15.00,15.00); -- 340
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (315,7,11,2,15.00,15.00); -- 341
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (316,7,12,2,15.00,15.00); -- 342
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (317,7,13,2,15.00,15.00); -- 343
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (318,7,14,2,15.00,15.00); -- 344
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (319,7,15,2,15.00,15.00); -- 345
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (320,7,16,2,15.00,15.00); -- 346
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (321,7,17,2,15.00,15.00); -- 347
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (322,7,18,2,15.00,15.00); -- 348
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (323,7,19,2,15.00,15.00); -- 349
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (324,7,20,2,15.00,15.00); -- 350
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (325,7,21,2,15.00,15.00); -- 351
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (326,7,22,2,15.00,15.00); -- 352
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (327,7,23,2,15.00,15.00); -- 353
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (328,7,24,2,15.00,15.00); -- 354
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (329,7,25,2,15.00,15.00); -- 355
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (330,7,26,2,15.00,15.00); -- 356
-- --------------------------------------------
-- SERVICO - Debrum (feito á maquina)
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,1,18.00,18.00); -- 357
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,2,18.00,18.00); -- 358
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,3,18.00,18.00); -- 359
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,4,18.00,18.00); -- 360
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,5,18.00,18.00); -- 361
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,6,18.00,18.00); -- 362
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,7,18.00,18.00); -- 363
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,8,18.00,18.00); -- 364
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,9,18.00,18.00); -- 365
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,10,18.00,18.00); -- 366
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,11,18.00,18.00); -- 367
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,12,18.00,18.00); -- 368
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,13,18.00,18.00); -- 369
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,14,18.00,18.00); -- 370
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,15,18.00,18.00); -- 371
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,16,18.00,18.00); -- 372
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,17,18.00,18.00); -- 373
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,18,18.00,18.00); -- 374
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,19,18.00,18.00); -- 375
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,20,18.00,18.00); -- 376
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,21,18.00,18.00); -- 377
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,22,18.00,18.00); -- 378
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,23,18.00,18.00); -- 379
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,24,18.00,18.00); -- 380
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,25,18.00,18.00); -- 381
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (8,26,18.00,18.00); -- 382
-- SERVICO - Debrum (feito á maquina) - LAVAGEM
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (357,8,1,2,12.00,12.00); -- 383
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (358,8,2,2,12.00,12.00); -- 384
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (359,8,3,2,12.00,12.00); -- 385
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (360,8,4,2,12.00,12.00); -- 386
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (361,8,5,2,12.00,12.00); -- 387
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (362,8,6,2,12.00,12.00); -- 388
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (363,8,7,2,12.00,12.00); -- 389
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (364,8,8,2,12.00,12.00); -- 390
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (365,8,9,2,12.00,12.00); -- 391
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (366,8,10,2,12.00,12.00); -- 392
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (367,8,11,2,12.00,12.00); -- 393
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (368,8,12,2,12.00,12.00); -- 394
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (369,8,13,2,12.00,12.00); -- 395
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (370,8,14,2,12.00,12.00); -- 396
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (371,8,15,2,12.00,12.00); -- 397
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (372,8,16,2,12.00,12.00); -- 398
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (373,8,17,2,12.00,12.00); -- 399
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (374,8,18,2,12.00,12.00); -- 400
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (375,8,19,2,12.00,12.00); -- 401
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (376,8,20,2,12.00,12.00); -- 402
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (377,8,21,2,12.00,12.00); -- 403
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (378,8,22,2,12.00,12.00); -- 404
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (379,8,23,2,12.00,12.00); -- 405
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (380,8,24,2,12.00,12.00); -- 406
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (381,8,25,2,12.00,12.00); -- 407
INSERT INTO tb_valores_servicos ( cod_valor_servico_pai, cod_servico, cod_tapete, cod_tipo_cliente, val_inicial, val_acima_10m2 ) VALUES (382,8,26,2,12.00,12.00); -- 408
-- --------------------------------------------
-- SERVICO - Cola
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,1,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,2,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,3,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,4,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,5,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,6,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,7,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,8,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,9,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,10,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,11,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,12,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,13,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,14,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,15,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,16,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,17,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,18,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,19,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,20,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,21,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,22,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,23,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,24,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,25,0.00,0.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (9,26,0.00,0.00); -- 

-- --------------------------------------------
-- SERVICO - Forro de algodão
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,1,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,2,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,3,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,4,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,5,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,6,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,7,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,8,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,9,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,10,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,11,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,12,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,13,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,14,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,15,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,16,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,17,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,18,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,19,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,20,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,21,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,22,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,23,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,24,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,25,20.00,20.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (10,26,20.00,20.00); -- 

-- --------------------------------------------
-- SERVICO - Couro
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,1,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,2,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,3,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,4,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,5,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,6,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,7,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,8,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,9,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,10,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,11,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,12,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,13,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,14,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,15,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,16,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,17,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,18,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,19,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,20,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,21,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,22,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,23,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,24,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,25,30.00,30.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (11,26,30.00,30.00); -- 

-- --------------------------------------------
-- SERVICO - Goma em Sisal
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,1,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,2,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,3,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,4,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,5,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,6,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,7,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,8,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,9,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,10,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,11,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,12,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,13,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,14,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,15,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,16,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,17,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,18,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,19,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,20,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,21,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,22,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,23,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,24,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,25,5.00,5.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (12,26,5.00,5.00); -- 

-- --------------------------------------------
-- SERVICO - Antiderrapante
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,1,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,2,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,3,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,4,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,5,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,6,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,7,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,8,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,9,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,10,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,11,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,12,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,13,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,14,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,15,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,16,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,17,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,18,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,19,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,20,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,21,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,22,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,23,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,24,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,25,35.00,35.00); -- 
INSERT INTO tb_valores_servicos ( cod_servico, cod_tapete, val_inicial, val_acima_10m2 ) VALUES (13,26,35.00,35.00); -- 


CREATE TABLE tb_ordens_de_servico (
  cod_ordem_de_servico INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_cliente INTEGER UNSIGNED NOT NULL,
  cod_usuario INTEGER UNSIGNED NOT NULL,
  cod_status_os INTEGER UNSIGNED NOT NULL,
  num_os INTEGER UNSIGNED NOT NULL,
  val_original DOUBLE NOT NULL,
  val_final DOUBLE NOT NULL DEFAULT 0,
  dat_abertura TIMESTAMP NOT NULL DEFAULT '2001-01-01 00:00:00',
  dat_prev_conclusao TIMESTAMP NOT NULL DEFAULT '2001-01-01 00:00:00',
  dat_fechamento TIMESTAMP NULL,
  txt_observacoes TEXT NULL,
  dat_cadastro TIMESTAMP NOT NULL DEFAULT CURRENT_TIMESTAMP  ,
  dat_atualizacao TIMESTAMP NOT NULL DEFAULT '2001-01-01 00:00:00',
  PRIMARY KEY(cod_ordem_de_servico),
  INDEX ix_cod_status_os(cod_status_os),
  INDEX ix_cod_cliente(cod_cliente),
  INDEX ix_cod_usuario(cod_usuario),
  INDEX ix_num_os(num_os),
  FOREIGN KEY(cod_status_os)
    REFERENCES tb_status_os(cod_status_os)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_cliente)
    REFERENCES tb_clientes(cod_cliente)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_usuario)
    REFERENCES tb_usuarios(cod_usuario)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_cidades (
  cod_cidade INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_cidade VARCHAR(50) NOT NULL,
  cod_estado INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY(cod_cidade),
  INDEX ix_cod_estado(cod_estado),
  FOREIGN KEY(cod_estado)
    REFERENCES tb_estados(cod_estado)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_bairros (
  cod_bairro INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_bairro VARCHAR(50) NOT NULL,
  cod_cidade INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY(cod_bairro),
  INDEX ix_cod_cidade(cod_cidade),
  FOREIGN KEY(cod_cidade)
    REFERENCES tb_cidades(cod_cidade)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_itens_os (
  cod_item_os INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_tapete INTEGER UNSIGNED NOT NULL,
  cod_ordem_de_servico INTEGER UNSIGNED NOT NULL,
  flt_comprimento FLOAT NOT NULL,
  flt_largura FLOAT NOT NULL,
  dbl_area DOUBLE NOT NULL,
  val_item DOUBLE NOT NULL,
  txt_observacoes TEXT NULL,
  PRIMARY KEY(cod_item_os),
  INDEX ix_cod_ordem_de_servico(cod_ordem_de_servico),
  INDEX ix_cod_tapete(cod_tapete),
  FOREIGN KEY(cod_ordem_de_servico)
    REFERENCES tb_ordens_de_servico(cod_ordem_de_servico)
      ON DELETE CASCADE
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_tapete)
    REFERENCES tb_tapetes(cod_tapete)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_logradouros (
  cod_logradouro INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  nom_logradouro VARCHAR(50) NOT NULL,
  txt_cep VARCHAR(10) NOT NULL,
  cod_bairro INTEGER UNSIGNED NOT NULL,
  cod_tipo_logradouro INTEGER UNSIGNED NOT NULL,
  PRIMARY KEY(cod_logradouro),
  INDEX ix_cod_bairro(cod_bairro),
  INDEX ix_cod_tipo_ligradouro(cod_tipo_logradouro),
  FOREIGN KEY(cod_bairro)
    REFERENCES tb_bairros(cod_bairro)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_tipo_logradouro)
    REFERENCES tb_tipos_logradouros(cod_tipo_logradouro)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_itens_servicos (
  cod_item_servico INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_item_os INTEGER UNSIGNED NOT NULL,
  cod_servico INTEGER UNSIGNED NOT NULL,
  qtd_m_m2 DOUBLE NOT NULL,
  val_item_servico DOUBLE NOT NULL,
  PRIMARY KEY(cod_item_servico),
  INDEX ix_cod_item_os(cod_item_os),
  INDEX ix_cod_servico(cod_servico),
  FOREIGN KEY(cod_item_os)
    REFERENCES tb_itens_os(cod_item_os)
      ON DELETE CASCADE
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_servico)
    REFERENCES tb_servicos(cod_servico)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

CREATE TABLE tb_enderecos (
  cod_endereco INTEGER UNSIGNED NOT NULL AUTO_INCREMENT,
  cod_cliente INTEGER UNSIGNED NOT NULL,
  cod_logradouro INTEGER UNSIGNED NOT NULL,
  txt_complemento VARCHAR(255) NULL,
  txt_ponto_referencia VARCHAR(255) NULL,
  int_numero INTEGER UNSIGNED NULL,
  PRIMARY KEY(cod_endereco),
  INDEX ix_cod_logradouro(cod_logradouro),
  INDEX ix_cod_cliente(cod_cliente),
  FOREIGN KEY(cod_logradouro)
    REFERENCES tb_logradouros(cod_logradouro)
      ON DELETE NO ACTION
      ON UPDATE NO ACTION,
  FOREIGN KEY(cod_cliente)
    REFERENCES tb_clientes(cod_cliente)
      ON DELETE CASCADE
      ON UPDATE NO ACTION
)
ENGINE=InnoDB;

