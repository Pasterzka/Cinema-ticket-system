import psycopg2

# Database configuration
DB_CONFIG = {
    "host": "localhost",
    "port": 5432,
    "dbname": "CineQueueDb",
    "user": "admin",
    "password": "admin"
}

def clear_database():
    print("Clearing the database...")
    try:
        # Connect to the PostgreSQL database
        connection = psycopg2.connect(**DB_CONFIG)
        cursor = connection.cursor()

        cursor.execute("""
            TRUNCATE TABLE "Reservations" CASCADE;
            TRUNCATE TABLE "Seats" CASCADE;
            TRUNCATE TABLE "ShowTimes" CASCADE;
            TRUNCATE TABLE "Halls" CASCADE;
            TRUNCATE TABLE "Movies" CASCADE;            
        """)

        connection.commit()
        print("Database cleared successfully.")

        cursor.close()
        connection.close()
    except Exception as e:
        print(f"Error clearing the database: {e}")

if __name__ == "__main__":
    clear_database()