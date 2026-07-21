import psycopg2
import uuid
import random
from datetime import datetime, timedelta
from faker import Faker

# Faker and database configuration
fake = Faker('pl_PL')

DB_CONFIG = {
    "host": "localhost",
    "port": 5432,
    "dbname": "CineQueueDb",
    "user": "admin",
    "password": "admin"
}

# Constants for data generation
NUM_MOVIES = 5
NUM_HALLS = 3
SEATS_PER_ROW = 5
ROWS_PER_HALL = 5
RESERVATION_CHANCE = 0.25   # 25% chance of a seat being reserved

def generate_database():
    print("Generating database...")

    try:
        connection = psycopg2.connect(**DB_CONFIG)
        cursor = connection.cursor()


        print("Generating movies...")
        movies = []
        movies_titles = ["Inception", "The Matrix", "Interstellar", "The Dark Knight", "Pulp Fiction"]
        for title in movies_titles:
            movie_id = str(uuid.uuid4())
            description = fake.text(max_nb_chars=124)
            duration = random.randint(90, 180)  # Duration between 90 and 180 minutes
            cursor.execute("INSERT INTO \"Movies\" (\"Id\", \"Title\", \"Description\", \"Duration\") VALUES (%s, %s, %s, %s)", (movie_id, title, description, duration))
            movies.append(movie_id)

        print("Generating halls...")
        halls = []
        for i in range(1, NUM_HALLS + 1):
            hall_id = str(uuid.uuid4())
            hall_name = f"Hall {i}" if i < NUM_HALLS else "VIP Hall"
            cursor.execute("INSERT INTO \"Halls\" (\"Id\", \"Name\") VALUES (%s, %s)", (hall_id, hall_name))
            halls.append(hall_id)

        print("Generating showtimes, rows and reservations...")
        now = datetime.now()
        reservation_count = 0
        for movie_id in movies:
            for _ in range(random.randint(1,3)):

                showtime_id = str(uuid.uuid4())
                hall_id = random.choice(halls)

                start_time = now + timedelta(days=random.randint(0, 3))
                start_time = start_time.replace(hour=random.randint(10, 22), minute=random.choice([0, 15, 30, 45]), second=0, microsecond=0)

                cursor.execute("INSERT INTO \"ShowTimes\" (\"Id\", \"MovieId\", \"HallId\", \"StartTime\") VALUES (%s, %s, %s, %s)", (showtime_id, movie_id, hall_id, start_time))

                for row in range(1, ROWS_PER_HALL + 1):
                    for seat_number in range(1, SEATS_PER_ROW + 1):
                        seat_id = str(uuid.uuid4())

                        is_reserved = random.random() < RESERVATION_CHANCE
                        status = 'Reserved' if is_reserved else 'Free'

                        cursor.execute("INSERT INTO \"Seats\" (\"Id\", \"ShowTimeId\", \"Row\", \"Number\", \"Status\") VALUES (%s, %s, %s, %s, %s)", (seat_id, showtime_id, row, seat_number, status))

                        if is_reserved:
                            reservation_id = str(uuid.uuid4())
                            customer_email = fake.email()

                            cursor.execute("INSERT INTO \"Reservations\" (\"Id\", \"SeatId\", \"CustomerEmail\", \"ReservationTime\") VALUES (%s, %s, %s, %s)", (reservation_id, seat_id, customer_email, datetime.now()))
                            reservation_count += 1


        connection.commit()
        print(f"Database generated successfully with {len(movies)} movies, {len(halls)} halls, and {reservation_count} reservations.")

        cursor.close()
        connection.close()
    except Exception as e:
        print(f"Error generating the database: {e}")

if __name__ == "__main__":
    generate_database()

    