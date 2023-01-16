create database anova_db;
\c anova_db;

create table devices (
    device_id int not null primary key,
    name text not null,
    location text not null
);

create table readings (
    timestamp bigint not null,
    device_id int not null,
    raw_value text not null,
    reading_type text not null,
    foreign key (device_id) references devices(device_id) on delete cascade
);

insert into devices (device_id, name, location) values
(1, 'great machine 1', 'warsaw'),
(234, 'great machine 2', 'porto');