﻿/* Copyright 2014 Michael Braude and individual contributors.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Threading.Tasks;
using OneBusAway.Model;
using OneBusAway.Services;
using Windows.Devices.Geolocation;

namespace OneBusAway.Platforms.WindowsPhone
{
    /// <summary>
    /// Finds the user's location.
    /// </summary>
    public class GeoLocationService : IGeoLocationService
    {
        /// <summary>
        /// This is the geo locator object that's used to find our location.
        /// </summary>
        private GeoCoordinateWatcher geoLocator;

        /// <summary>
        /// A list of weak references to location observers.
        /// </summary>
        private List<WeakReference<IGeoLocationServiceObserver>> observers;

        /// <summary>
        /// Creates the service.
        /// </summary>
        public GeoLocationService()
        {
            this.observers = new List<WeakReference<IGeoLocationServiceObserver>>();

            this.geoLocator = new GeoCoordinateWatcher(GeoPositionAccuracy.High);
            this.geoLocator.Start();
            this.geoLocator.PositionChanged += OnGeoLocatorPositionChanged;
        }

        /// <summary>
        /// Fire the OnUserLocationChanged event if somebody has subscribed.
        /// </summary>
        private void OnGeoLocatorPositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate coordinate = e.Position.Location;
            Point newLocation = new Point(coordinate.Latitude, coordinate.Longitude);

            // Count backwards so that we can remove references from the list:
            for (int i = this.observers.Count - 1; i >= 0; i--)
            {
                IGeoLocationServiceObserver observer;
                if (observers[i].TryGetTarget(out observer))
                {
                    observer.OnUserLocationChanged(newLocation);
                }
                else
                {
                    observers.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Registers the observer with the list of observers to update.
        /// </summary>
        public void RegisterForLocationChanged(IGeoLocationServiceObserver observer)
        {
            this.observers.Add(new WeakReference<IGeoLocationServiceObserver>(observer));
        }

        /// <summary>
        /// Finds the user location.
        /// </summary>
        /// <returns></returns>
        public Task<Point> FindUserLocationAsync()
        {
            var location = this.geoLocator.Position.Location;
            return Task.FromResult<Point>(new Point(location.Latitude, location.Longitude));
        }
    }
}
