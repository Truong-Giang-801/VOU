import 'dart:convert';
import 'package:http/http.dart' as http;

// Brand Model
class Brand {
  final int id;
  final String name;
  final String gps;
  final String industry;
  final DateTime dateCreated;
  final DateTime dateUpdated;

  Brand({
    required this.id,
    required this.name,
    required this.gps,
    required this.industry,
    required this.dateCreated,
    required this.dateUpdated,
  });

  factory Brand.fromJson(Map<String, dynamic> json) {
    return Brand(
      id: json['id'],
      name: json['name'],
      gps: json['gps'],
      industry: json['industry'],
      dateCreated: DateTime.parse(json['dateCreated']),
      dateUpdated: DateTime.parse(json['dateUpdated']),
    );
  }

  Map<String, dynamic> toJson() {
    return {
      'id': id,
      'name': name,
      'gps': gps,
      'industry': industry,
      'dateCreated': dateCreated.toIso8601String(),
      'dateUpdated': dateUpdated.toIso8601String(),
    };
  }
}


