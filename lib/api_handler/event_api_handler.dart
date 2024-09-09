import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:vou_web/class/event.dart'; // Event class

const String baseUrl = 'https://localhost:7003/api/Event'; // Replace with your actual API URL

// Get all events by brandId
Future<List<Event>> getEventsByBrandID(String brandID) async {
  final response = await http.get(Uri.parse('$baseUrl/brand/$brandID'));

  if (response.statusCode == 200) {
    List<dynamic> body = jsonDecode(response.body)['result'];
    return body.map((dynamic item) => Event.fromJson(item)).toList();
  } else {
    throw Exception('Failed to load events');
  }
}

// Add new event
Future<bool> addEvent(Event event) async {
  final response = await http.post(
    Uri.parse(baseUrl),
    headers: {"Content-Type": "application/json"},
    body: jsonEncode(event.toJson()),
  );

  return response.statusCode == 200;
}

// Update existing event
Future<bool> updateEvent(Event event) async {
  final response = await http.put(
    Uri.parse('$baseUrl/${event.id}'),
    headers: {"Content-Type": "application/json"},
    body: jsonEncode(event.toJson()),
  );

  return response.statusCode == 200;
}

// Delete event by id
Future<bool> deleteEvent(int id) async {
  final response = await http.delete(Uri.parse('$baseUrl/$id'));

  return response.statusCode == 200;
}
