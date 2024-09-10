import 'dart:convert';
import 'package:http/http.dart' as http;
import 'package:vou_web/class/event.dart'; // Import your Event model

const String baseUrl = 'https://localhost:7003/api/Event'; // Replace with your actual API URL

// Get all events
Future<List<Event>> getAllEvents() async {
  final response = await http.get(Uri.parse(baseUrl));

  if (response.statusCode == 200) {
    List<dynamic> body = jsonDecode(response.body)['result'];
    return body.map((dynamic item) => Event.fromJson(item)).toList();
  } else {
    throw Exception('Failed to load events');
  }
}

// Get event by ID
Future<Event> getEventById(int id) async {
  final response = await http.get(Uri.parse('$baseUrl/brand/$id'));

  if (response.statusCode == 200) {
    final responseData = jsonDecode(response.body);
    return Event.fromJson(responseData['result']);
  } else {
    throw Exception('Failed to load event');
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

// Delete event by ID
Future<bool> deleteEvent(int id) async {
  final response = await http.delete(Uri.parse('$baseUrl/$id'));

  return response.statusCode == 200;
}

// Get events by brand ID
Future<List<Event>> getEventsByBrandID(int brandId) async {
  final response = await http.get(Uri.parse('$baseUrl/brand/$brandId'));

  if (response.statusCode == 200) {
    List<dynamic> body = jsonDecode(response.body)['result'];
    return body.map((dynamic item) => Event.fromJson(item)).toList();
  } else {
    throw Exception('Failed to load events by brand ID');
  }
}
