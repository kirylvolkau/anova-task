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

insert into readings (timestamp, device_id, raw_value, reading_type) values
(1674150492, 1, '80', 'battery'),
(1642610892, 1, '92', 'battery'),
(1647708492, 1, '90', 'battery'),
(1666194492, 1, '55', 'battery'),
(1666194492, 234, '44', 'tank'),
(1664194492, 234, '33', 'tank');
